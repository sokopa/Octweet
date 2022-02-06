using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Extensions;
using Octweet.Data.Extensions;
using Octweet.Data;
using Serilog;

namespace Octweet.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            var dbContext = host.Services.GetRequiredService<OctweetDbContext>();
            dbContext.Database.EnsureCreated();

            await host.RunAsync();
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
                    serviceCollection.AddDataServices(configurationRoot.GetConnectionString("OctweetDB"), typeof(Program).Assembly.FullName);
                })
                .UseSerilog();
        }
    }
}
