using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octweet.ConsoleApp.Configuration;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Services;
using System;
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

                    serviceCollection.AddTransient<TwitterService>();
                });
        }
    }
}
