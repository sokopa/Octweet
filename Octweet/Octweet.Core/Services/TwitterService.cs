using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Abstractions.Services;
using Octweet.Data.Abstractions.Repositories;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Octweet.Core.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly ITwitterClient _twitterClient;
        private readonly TwitterClientConfig _configuration;
        private readonly IQueryLogRepository _queryLogRepository;
        private readonly ILogger<TwitterService> _logger;
        private readonly ITweetRepository _tweetRepository;

        public TwitterService(
            TwitterClientConfig configuration, 
            ITweetRepository tweetRepository,
            IQueryLogRepository queryLogRepository, 
            ILogger<TwitterService> logger)
        {
            _configuration = configuration;
            _tweetRepository = tweetRepository;
            _queryLogRepository = queryLogRepository;
            _logger = logger;
            var credentials = new ConsumerOnlyCredentials(_configuration.ApiKey, _configuration.ApiSecret, _configuration.BearerToken);
            _twitterClient = new TwitterClient(credentials);
        }

        public ITwitterClient TwitterClient => _twitterClient;

        public async Task<IEnumerable<string>> GetTweetIdsWithImages()
        {
            var appendedWithImageFilter = _configuration.Query + " has:images";
            var parameters = new SearchTweetsV2Parameters(appendedWithImageFilter);
            var tweetsByQuery = await TwitterClient.SearchV2.SearchTweetsAsync(parameters);
            var imageUrls = tweetsByQuery.Includes.Media.Where(m => m.Type == "photo").Select(m => m.Url);

            return imageUrls;
        }

        public async Task QueryLatestTweets()
        {
            var queryToExecute = _configuration.Query + " has:images";

            _logger.LogInformation("Start querying latest tweets with query: {query}", queryToExecute);

            var parameters = new SearchTweetsV2Parameters(queryToExecute);

            // logic to only fetch new tweets after each run
            var latestExecutionForQuery = await _queryLogRepository.GetLatestExecution(queryToExecute);
            if (latestExecutionForQuery != null)
            {
                parameters.SinceId = latestExecutionForQuery.LatestTweetId;
                _logger.LogDebug("Already fetched tweets up to {tweetId}. Using SinceId.", latestExecutionForQuery.LatestTweetId);
            }

            List<SearchTweetsV2Response> tweetResponsePages = new List<SearchTweetsV2Response>();
            try
            {
                var pagingSearchIterator = TwitterClient.SearchV2.GetSearchTweetsV2Iterator(parameters);

                int pageIndex = 0;
                while (!pagingSearchIterator.Completed)
                {
                    _logger.LogInformation("Processing page {page}...", ++pageIndex);
                    var page = await pagingSearchIterator.NextPageAsync();
                    _logger.LogDebug("Query for page {page} returned {count} tweets", pageIndex, page.Content.Tweets.Count());
                    tweetResponsePages.Add(page.Content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching tweets");
                throw;
            }

            if (!tweetResponsePages.Any() || tweetResponsePages.FirstOrDefault().Tweets.Count() == 0) 
                return;

            var latestTweetId = tweetResponsePages.SelectMany(i => i.Tweets).OrderByDescending(t => t.CreatedAt).First().Id;
            
            var tweetsToSave = MapToTweetModel(tweetResponsePages);

            try
            {
                await _tweetRepository.SaveTweets(tweetsToSave);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while saving tweets");
                throw;
            }

            if (latestExecutionForQuery == null)
            {
                latestExecutionForQuery = new Data.Abstractions.QueryLog
                {
                    Query = queryToExecute
                };
            }

            latestExecutionForQuery.LatestTweetId = latestTweetId;
            latestExecutionForQuery.LatestExecution = DateTime.UtcNow;

            await _queryLogRepository.InsertOrUpdateQueryLog(latestExecutionForQuery);
        }

        private IEnumerable<Octweet.Data.Abstractions.Tweet> MapToTweetModel(IEnumerable<SearchTweetsV2Response> responses)
        {
            foreach (var response in responses)
                foreach (var tweet in response.Tweets)
                {
                    if (tweet.Attachments == null || tweet.Attachments.MediaKeys.Length == 0)
                    {
                        continue;
                    }

                    var keys = tweet.Attachments.MediaKeys;
                    var media = response.Includes.Media
                        .Where(m => m.Type == "photo")
                        .Where(m => keys.Contains(m.MediaKey));

                    var mappedTweet = new Data.Abstractions.Tweet
                    {
                        Id = tweet.Id,
                        CreatedAt = tweet.CreatedAt,
                        AuthorId = tweet.AuthorId,
                        Language = tweet.Lang,
                        Text = tweet.Text
                    };
                    var mappedMedia = media.Select(m => new Data.Abstractions.TweetMedia
                    {
                        Height = m.Height,
                        Width = m.Width,
                        MediaKey = m.MediaKey,
                        Tweet = mappedTweet,
                        Type = m.Type,
                        Url = m.Url
                    }).ToList();
                    mappedTweet.Media = mappedMedia;

                    yield return mappedTweet;
                }
        }
    }
}
