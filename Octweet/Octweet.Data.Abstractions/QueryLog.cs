using System;

namespace Octweet.Data.Abstractions
{
    public class QueryLog
    {
        public int Id { get; set; }
        public string Query { get; set; }
        public string LatestTweetId { get; set; }
        public DateTime LatestExecution { get; set; } = DateTime.UtcNow;
    }
}
