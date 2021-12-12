using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Octweet.Data.Abstractions.Repositories
{
    public interface ITweetRepository
    {
        Task SaveTweets(IEnumerable<Tweet> tweets);
        Task<IEnumerable<TweetMedia>> FetchUnprocessedTweetMedia(int batchSize);
    }

    public interface IAnnotationRepository
    {
        Task SaveAnnotationResults(EntityAnnotation annotation);
    }

    public interface IQueryLogRepository
    {
        Task<QueryLog> GetLatestExecution(string query);
        Task InsertOrUpdateQueryLog(QueryLog queryLog);
    }
}
