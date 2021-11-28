namespace Octweet.Data.Abstractions
{
    public class EntityAnnotation
    {
        public int Id { get; set; }
        public TweetMedia TweetMedia { get; set; }
        public string MediaKey { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
    }
}
