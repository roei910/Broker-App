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
            //using PeriodicTimer timer = new PeriodicTimer(_period);
            //while (!stoppingToken.IsCancellationRequested &&
            //       await timer.WaitForNextTickAsync(stoppingToken))
            //{
            //    await Task.Delay(1000);
            //    await UpdateStockPrice(stoppingToken);
            //}
        }

        private async Task UpdateStockPrice(CancellationToken stoppingToken)
        {
            //db.Stocks.Find(1).priceHistory.Append(1);
            //_logger.LogInformation("Executing PeriodicBackgroundTask");

            foreach (var stock in db.Stocks)
            {
                //stock.CurrentPrice = Convert.ToInt32(Math.Ceiling(stock.CurrentPrice * 1.01));
                await db.SaveChangesAsync(stoppingToken);

            }
        }
    }
}
