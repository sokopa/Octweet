using System;
using System.Collections.Generic;

namespace Octweet.Data.Abstractions
{
    public class Tweet
    {
        public string Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }

        public string Language { get; set; }

        public ICollection<TweetMedia> Media { get; set; }
    }
}
