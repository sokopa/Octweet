using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Microsoft.Extensions.Logging;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Abstractions.Services;
using Octweet.Data.Abstractions.Repositories;

namespace Octweet.Core.Services
{
    public class GoogleVisionService : IAnnotationService
    {
        private readonly ILogger<GoogleVisionService> _logger;
        private readonly ITweetRepository _tweetRepository;
        private readonly IAnnotationRepository _annotationRepository;
        private readonly GoogleClientConfig _googleClientConfig;
        public ImageAnnotatorClient ImageAnnotatorClient { get; private set; }

        public GoogleVisionService(
            GoogleClientConfig config,
            ITweetRepository tweetRepository,
            IAnnotationRepository annotationRepository,
            ILogger<GoogleVisionService> logger
            )
        {
            _googleClientConfig = config;
            ImageAnnotatorClient = new ImageAnnotatorClientBuilder
            {
                CredentialsPath = _googleClientConfig.VisionCredentialsPath
            }.Build();
            _tweetRepository = tweetRepository;
            _annotationRepository = annotationRepository;
            _logger = logger;
        }

        public async Task AnnotatePendingTweetMedia()
        {
            var pendingMedia = await _tweetRepository.FetchUnprocessedTweetMedia(_googleClientConfig.VisionBatchSize);
            if (pendingMedia == null || pendingMedia.Count() == 0)
            {
                _logger.LogInformation("No pending tweets needing OCR found.");
                return;
            }
            var runDescription = $"{pendingMedia.FirstOrDefault().Id}-{pendingMedia.LastOrDefault().Id}";

            _logger.LogInformation($"Start processing {pendingMedia.Count()} pending media images. Batch: {runDescription}");
            Data.Abstractions.EntityAnnotation annotation = null;
            foreach (var media in pendingMedia)
            {
                var img = Image.FromUri(media.Url);
                try
                {

                    var ocrResults = await ImageAnnotatorClient.DetectDocumentTextAsync(img);
                    
                    if (ocrResults == null)
                    {
                        // no text exists in the image
                        annotation = CreateNoTextAnnotation(media.Id);
                    }
                    else
                    {
                        annotation = CreateAnnotationFromOcrResult(media.Id, ocrResults);
                    }

                    await _annotationRepository.SaveAnnotationResults(annotation);
                }
                catch(AnnotateImageException ex)
                {
                    _logger?.LogError(ex, "Error in Google Vision Annotation");
                }
                catch(Exception ex)
                {
                    _logger?.LogError(ex, "Error in Google Vision Service");
                }
            }
            _logger.LogInformation($"Done processing {pendingMedia.Count()} pending media images. Batch: {runDescription}");
        }

        private Data.Abstractions.EntityAnnotation CreateAnnotationFromOcrResult(int tweetMediaId, TextAnnotation textAnnotation)
        {
            // store the first result only (the full text summary)
            var result = textAnnotation.Pages;
            var allText = textAnnotation.Text;
            var detectedLanguage = result.First().Property.DetectedLanguages.FirstOrDefault();
            var annotation = new Data.Abstractions.EntityAnnotation
            {
                Description = allText,
                Locale = detectedLanguage.LanguageCode,
                TweetMediaId = tweetMediaId,
                ContainsText = true
            };
            return annotation;
        }

        private Data.Abstractions.EntityAnnotation CreateNoTextAnnotation(int tweetMediaId, string description = null)
        {
            description = description ?? "No text detected";
            var annotation = new Data.Abstractions.EntityAnnotation
            {
                Description = description,
                Locale = "en",
                TweetMediaId = tweetMediaId,
                ContainsText = false
            };
            return annotation;
        }
    }
}
