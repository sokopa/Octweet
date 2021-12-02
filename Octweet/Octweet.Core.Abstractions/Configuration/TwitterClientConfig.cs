namespace Octweet.Core.Abstractions.Configuration
{
    public class TwitterClientConfig
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string BearerToken  { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public string Query { get; set; }
        public int PollingPeriodInSeconds { get; set; }
    }
}
