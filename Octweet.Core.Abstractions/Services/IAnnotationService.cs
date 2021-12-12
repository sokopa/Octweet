using System.Threading.Tasks;

namespace Octweet.Core.Abstractions.Services
{
    public interface IAnnotationService
    {
        Task AnnotatePendingTweetMedia();
    }
}
