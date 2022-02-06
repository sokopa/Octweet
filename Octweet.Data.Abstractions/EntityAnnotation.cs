namespace Octweet.Data.Abstractions
{
    public class EntityAnnotation
    {
        public int Id { get; set; }
        public int TweetMediaId { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
        public bool? ContainsText { get; set; }
    }
}
