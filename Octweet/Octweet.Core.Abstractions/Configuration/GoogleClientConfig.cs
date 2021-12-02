namespace Octweet.Core.Abstractions.Configuration
{
    public class GoogleClientConfig
    {
        public string VisionCredentialsPath { get; set; }
        public int PollingPeriodInSeconds { get; set; } = 20;
        public int VisionBatchSize { get; set; } = 20;
    }
}
