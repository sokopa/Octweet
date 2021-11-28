using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octweet.Data.Abstractions;

namespace Octweet.Data.Configuration
{
    public class EntityAnnotationEntityTypeConfiguration : IEntityTypeConfiguration<EntityAnnotation>
    {
        public void Configure(EntityTypeBuilder<EntityAnnotation> builder)
        {
            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.HasKey(a => a.Id);

            builder.HasOne<TweetMedia>(a => a.TweetMedia)
                .WithOne(m => m.Annotation)
                .HasForeignKey<TweetMedia>(a => a.MediaKey);
        }
    }
}
