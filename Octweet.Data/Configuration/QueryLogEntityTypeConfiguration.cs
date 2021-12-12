using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Octweet.Data.Abstractions;

namespace Octweet.Data.Configuration
{
    public class QueryLogEntityTypeConfiguration : IEntityTypeConfiguration<QueryLog>
    {
        public void Configure(EntityTypeBuilder<QueryLog> builder)
        {
            builder.Property(q => q.Id)
                .ValueGeneratedOnAdd();
            builder.HasKey(q => q.Id);
        }
    }
}
