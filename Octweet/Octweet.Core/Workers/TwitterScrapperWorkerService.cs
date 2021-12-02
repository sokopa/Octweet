using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octweet.Core.Abstractions.Configuration;
using Octweet.Core.Abstractions.Services;

namespace Octweet.Core.Workers
{
    public class TwitterScrapperWorkerService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TwitterScrapperWorkerService> _logger;
        private Timer _timer = null!;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly TimeSpan _timerPeriod;

        public TwitterScrapperWorkerService(IServiceProvider serviceProvider, ILogger<TwitterScrapperWorkerService> logger)
        {
            _serviceProvider = serviceProvider;
            var twitterConfig = _serviceProvider.GetRequiredService<TwitterClientConfig>();
            _timerPeriod = TimeSpan.FromSeconds(twitterConfig.PollingPeriodInSeconds);
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Twitter Scrapper Worker Service is running.");

            _timer = new Timer(ExecuteTask, null, _timerPeriod, TimeSpan.FromMilliseconds(-1));

            // also immediately execute on startup
            ExecuteTask(null);

            return Task.CompletedTask;   
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }


        private void ExecuteTask(object state)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = ExecuteAsync(_stoppingCts.Token);
        }

        private async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var twitterService = _serviceProvider.GetRequiredService<ITwitterService>();

            await twitterService.QueryLatestTweets();

            // restore execution
            _timer.Change(_timerPeriod, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
            _timer?.Dispose();
        }
    }
}
