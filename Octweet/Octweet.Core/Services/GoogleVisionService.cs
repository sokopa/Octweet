using Google.Cloud.Vision.V1;
using Octweet.Core.Abstractions.Configuration;
using System.Threading.Tasks;

namespace Octweet.Core.Services
{
    public class GoogleVisionService
    {
        public ImageAnnotatorClient ImageAnnotatorClient { get; private set; }

        public GoogleVisionService(GoogleClientConfig config)
        {
            ImageAnnotatorClient = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = config.VisionCredentialsPath
            }.Build();
        }
    }
}
