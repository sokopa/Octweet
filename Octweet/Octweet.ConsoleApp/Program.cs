using Google.Cloud.Vision.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Services;
using Octweet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Octweet.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            // Construct twitter client from DI
            var twitterService = serviceProvider.GetRequiredService<TwitterService>();

            var imageUrls = await twitterService.GetTweetIdsWithImages("#frontpages");
            var googleVisionImages = imageUrls.Select(url => Image.FromUri(url));

            var visionService = serviceProvider.GetRequiredService<GoogleVisionService>();

            var ocrResults = await visionService.ImageAnnotatorClient.DetectTextAsync(googleVisionImages.First());
            foreach (EntityAnnotation text in ocrResults)
            {
                Console.WriteLine($"Description: {text.Description}");
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

                    var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                        devEnvironmentVariable.ToLower() == "development";
                    if (isDevelopment)
                    {
                        configuration.AddUserSecrets<Program>();
                    }
                })
                .ConfigureServices((hostingContext, serviceCollection) =>
                {
                    var configurationRoot = hostingContext.Configuration;

                    // Load and inject Twitter client configuration
                    TwitterClientConfig twitterConfig = new();
                    configurationRoot.GetSection("Twitter")
                        .Bind(twitterConfig);
                    serviceCollection.AddSingleton<TwitterClientConfig>(twitterConfig);

                    // Load and inject Google client configuration
                    GoogleClientConfig googleConfig = new();
                    configurationRoot.GetSection("Google")
                        .Bind(googleConfig);
                    serviceCollection.AddSingleton<GoogleClientConfig>(googleConfig);

                    // Register Core Services
                    serviceCollection.AddTransient<TwitterService>();
                    serviceCollection.AddTransient<GoogleVisionService>();

                    // Register EF
                    serviceCollection.AddDbContext<OctweetDbContext>(
                        options => options.UseSqlServer("name=ConnectionStrings:OctweetDB", b => b.MigrationsAssembly(typeof(Program).Assembly.FullName)));
                });
        }
    }
}
