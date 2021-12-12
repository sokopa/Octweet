using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Octweet.Data.Abstractions;
using Octweet.Data.Abstractions.Repositories;

namespace Octweet.Data.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private readonly OctweetDbContext _context;

        public TweetRepository(OctweetDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<TweetMedia>> FetchUnprocessedTweetMedia(int batchSize)
        {
            var tweets = await _context.TweetsMedia
                .Where(m => m.ProcessedAt == null)
                .AsNoTracking()
                .Take(batchSize)
                .ToListAsync();

            return tweets;
        }

        public async Task SaveTweets(IEnumerable<Tweet> tweets)
        {
            await _context.Tweets.AddRangeAsync(tweets);
            await _context.SaveChangesAsync();
        }
    }
}
