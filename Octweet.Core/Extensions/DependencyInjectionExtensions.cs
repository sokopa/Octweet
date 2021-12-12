using Microsoft.Extensions.DependencyInjection;
using Octweet.Core.Abstractions.Services;
using Octweet.Core.Services;
using Octweet.Core.Workers;

namespace Octweet.Core.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ITwitterService, TwitterService>();
            services.AddTransient<IAnnotationService, GoogleVisionService>();

            services.AddHostedService<TwitterScrapperWorkerService>();
            services.AddHostedService<GoogleVisionWorkerService>();

            return services;
        }
    }
}
