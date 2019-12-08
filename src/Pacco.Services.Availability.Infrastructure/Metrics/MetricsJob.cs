using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Gauge;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MetricsOptions = Convey.Metrics.AppMetrics.MetricsOptions;

namespace Pacco.Services.Availability.Infrastructure.Metrics
{
    public class MetricsJob : BackgroundService
    {
        private readonly GaugeOptions _threads = new GaugeOptions
        {
            Name = "threads"
        };

        private readonly GaugeOptions _workingSet = new GaugeOptions
        {
            Name = "working_set"
        };

        private readonly ILogger<MetricsJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly MetricsOptions _options;

        public MetricsJob(IServiceScopeFactory serviceScopeFactory, MetricsOptions options, ILogger<MetricsJob> logger)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.Enabled)
            {
                _logger.LogInformation("Metrics are disabled.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _logger.LogTrace("Processing metrics...");
                    var metricsRoot = scope.ServiceProvider.GetRequiredService<IMetricsRoot>();
                    var process = Process.GetCurrentProcess();
                    metricsRoot.Measure.Gauge.SetValue(_threads, process.Threads.Count);
                    metricsRoot.Measure.Gauge.SetValue(_workingSet, process.WorkingSet64);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}