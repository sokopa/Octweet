using Octweet.Core.Abstractions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters.V2;

namespace Octweet.Core.Services
{
    public class TwitterService
    {
        private readonly ITwitterClient _twitterClient;

        public TwitterService(TwitterClientConfig configuration)
        {
            var credentials = new ConsumerOnlyCredentials(configuration.ApiKey, configuration.ApiSecret, configuration.BearerToken);
            _twitterClient = new TwitterClient(credentials);
        }

        public ITwitterClient TwitterClient => _twitterClient;

        public async Task<IEnumerable<string>> GetTweetIdsWithImages(string query)
        {
            var appendedWithImageFilter = query + " has:images";
            var parameters = new SearchTweetsV2Parameters(appendedWithImageFilter);
            var tweetsByQuery = await TwitterClient.SearchV2.SearchTweetsAsync(parameters);
            var imageUrls = tweetsByQuery.Includes.Media.Where(m => m.Type == "photo").Select(m => m.Url);

            return imageUrls;
        }
    }
}
