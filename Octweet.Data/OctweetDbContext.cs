using Microsoft.EntityFrameworkCore;
using Octweet.Data.Abstractions;
using Octweet.Data.Configuration;

namespace Octweet.Data
{
    public class OctweetDbContext : DbContext
    {
        public OctweetDbContext(DbContextOptions<OctweetDbContext> options)
            : base(options) 
        {
        }

        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<TweetMedia> TweetsMedia { get; set; }
        public DbSet<EntityAnnotation> EntityAnnotations { get; set; }
        public DbSet<QueryLog> QueryLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TweetEntityTypeConfiguration).Assembly);
        }
    }
}
