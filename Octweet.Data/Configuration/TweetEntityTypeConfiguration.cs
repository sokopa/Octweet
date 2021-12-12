using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octweet.Data.Abstractions;

namespace Octweet.Data.Configuration
{
    public class TweetEntityTypeConfiguration : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder
                .HasKey(t => t.Id);

            // Configure one-to-many relationship with TweetMedia
            builder
                .HasMany<TweetMedia>(t => t.Media)
                .WithOne(m => m.Tweet)
                .HasForeignKey(m => m.TweetId);  
        }
    }
}
