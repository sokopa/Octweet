using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Octweet.Data.Abstractions;
using Octweet.Data.Abstractions.Repositories;

namespace Octweet.Data.Repositories
{
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly OctweetDbContext _context;

        public AnnotationRepository(OctweetDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveAnnotationResults(EntityAnnotation annotation)
        {
            var tweetMediaEntity = await _context.TweetsMedia.FindAsync(annotation.TweetMediaId);
            var insertedAnnotation = _context.EntityAnnotations.Add(annotation);
            await _context.SaveChangesAsync();
            tweetMediaEntity.ProcessedAt = DateTime.UtcNow;
            tweetMediaEntity.AnnotationId = insertedAnnotation.Entity.Id;
            await _context.SaveChangesAsync();
        }
    }
}
