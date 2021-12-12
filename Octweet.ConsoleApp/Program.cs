using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Vision.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Abstractions.Services;
using Octweet.Core.Extensions;
using Octweet.Core.Services;
using Octweet.Data.Extensions;
using Serilog;

namespace Octweet.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();           
            await host.RunAsync();

            //using IServiceScope serviceScope = host.Services.CreateScope();
            //IServiceProvider serviceProvider = serviceScope.ServiceProvider;
            //// Construct twitter client from DI
            //var twitterService = serviceProvider.GetRequiredService<ITwitterService>();

            //await twitterService.QueryLatestTweets();

            //Console.ReadKey();
            //var googleVisionImages = imageUrls.Select(url => Image.FromUri(url));

            //var visionService = serviceProvider.GetRequiredService<GoogleVisionService>();

            //var ocrResults = await visionService.ImageAnnotatorClient.DetectTextAsync(googleVisionImages.First());
            //foreach (EntityAnnotation text in ocrResults)
            //{
            //    Console.WriteLine($"Description: {text.Description}");
            //}
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

                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configurationRoot)
                        .CreateLogger();

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

                    serviceCollection.AddCoreServices();
                    serviceCollection.AddDataServices("name=ConnectionStrings:OctweetDB", typeof(Program).Assembly.FullName);
                })
                .UseSerilog();
        }
    }
}
