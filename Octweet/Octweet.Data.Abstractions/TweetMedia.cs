using System;

namespace Octweet.Data.Abstractions
{
    public class TweetMedia
    {
        public string TweetId { get; set; }
        public Tweet Tweet { get; set; }
        public string MediaKey { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; } = null; // defaults to null if not processed yet

        public int AnnotationId { get; set; }
        public EntityAnnotation Annotation { get; set; }
    }
}
