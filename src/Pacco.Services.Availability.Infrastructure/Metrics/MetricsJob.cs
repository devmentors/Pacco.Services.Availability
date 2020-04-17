using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Counter;
using Microsoft.Extensions.Hosting;

namespace Pacco.Services.Availability.Infrastructure.Metrics
{
    public class MetricsJob : BackgroundService
    {
        private readonly IMetricsRoot _metricsRoot;

        private readonly CounterOptions _dummyCounter = new CounterOptions
        {
            Name = "dummy_counter"
        };

        public MetricsJob(IMetricsRoot  metricsRoot)
        {
            _metricsRoot = metricsRoot;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _metricsRoot.Measure.Counter.Increment(_dummyCounter);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}