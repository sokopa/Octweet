using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octweet.Data.Abstractions;

namespace Octweet.Data.Configuration
{
    public class EntityAnnotationEntityTypeConfiguration : IEntityTypeConfiguration<EntityAnnotation>
    {
        public void Configure(EntityTypeBuilder<EntityAnnotation> builder)
        {
            builder.Property(m => m.Id)
                .ValueGeneratedOnAdd();
            builder.Property(m => m.ContainsText)
                .HasDefaultValue(true);
            builder
                .HasKey(m => m.Id);
        }
    }
}
