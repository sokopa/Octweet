using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octweet.Data.Abstractions;

namespace Octweet.Data.Configuration
{
    public class TweetMediaTypeConfiguration : IEntityTypeConfiguration<TweetMedia>
    {
        public void Configure(EntityTypeBuilder<TweetMedia> builder)
        {
            builder
                .HasKey(m => m.MediaKey);

            builder
                .HasOne<EntityAnnotation>(m => m.Annotation)
                .WithOne(a => a.TweetMedia)
                .HasForeignKey<EntityAnnotation>(a => a.MediaKey);
        }
    }
}
