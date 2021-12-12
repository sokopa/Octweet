using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octweet.Data.Abstractions;

namespace Octweet.Data.Configuration
{
    public class TweetMediaTypeConfiguration : IEntityTypeConfiguration<TweetMedia>
    {
        public void Configure(EntityTypeBuilder<TweetMedia> builder)
        {
            builder.Property(m => m.Id)
                .ValueGeneratedOnAdd();
            builder
                .HasKey(m => m.Id);
        }
    }
}
