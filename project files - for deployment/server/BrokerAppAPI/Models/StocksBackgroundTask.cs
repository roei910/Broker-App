using BrokerAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BrokerAppAPI.Models
{
    public class StocksBackgroundTask : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(1);
        private readonly ILogger<StocksBackgroundTask> _logger;
        private readonly ApiContext db;

        public StocksBackgroundTask(ILogger<StocksBackgroundTask> logger,
            IServiceScopeFactory scopeFactory)
        { 
            _logger = logger;
            _scopeFactory = scopeFactory;
            db = scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<ApiContext>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        { 
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                await Task.Delay(60000);
                await UpdateStockPrice(stoppingToken);
            }
        }

        private async Task UpdateStockPrice(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Executing PeriodicBackgroundTask");
            var random = new Random();
            for (var i = 0; i < 3; i++)
            {
                var stock = db.Stocks.Find(random.Next(1, db.Stocks.Count() + 1));
                if(stock != null)
                    stock.CurrentPrice *= 1.001;
                await db.SaveChangesAsync(stoppingToken);
            }
                
            for (var i = 0; i < 3; i++)
            {
                var stock = db.Stocks.Find(random.Next(1, db.Stocks.Count() + 1));
                if (stock != null)
                    stock.CurrentPrice *= 0.999;
                await db.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
