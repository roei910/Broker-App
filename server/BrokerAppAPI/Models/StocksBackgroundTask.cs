namespace BrokerAppAPI.Models
{
    public class StocksBackgroundTask : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        private readonly ILogger<StocksBackgroundTask> _logger;

        public StocksBackgroundTask(ILogger<StocksBackgroundTask> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Executing PeriodicBackgroundTask");
            }
        }
    }
}
