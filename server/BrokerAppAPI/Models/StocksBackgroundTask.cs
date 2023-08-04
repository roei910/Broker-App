using BrokerAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BrokerAppAPI.Models
{
    public class StocksBackgroundTask : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(1);
        //private readonly ILogger<StocksBackgroundTask> _logger;
        private readonly ApiContext db;

        public StocksBackgroundTask(ILogger<StocksBackgroundTask> logger,
            IServiceScopeFactory scopeFactory)
        { 
            //_logger = logger;
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
            //    await UpdateStockPrice(stoppingToken);
            //}
        }

        private async Task UpdateStockPrice(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);
            //_logger.LogInformation("Executing PeriodicBackgroundTask");

            foreach (var stock in db.Stocks)
            {

                //stock.CurrentPrice = Convert.ToInt32(Math.Ceiling(stock.CurrentPrice * 1.01));
                //await db.SaveChangesAsync(stoppingToken);

            }

            //var requests = db.OpenRequests.Where(
            //    req => req.Purchase);
            //IDictionary<int, int> stockRequestCount = new Dictionary<int, int>();
            //foreach(
            //    var req in
            //    requests)
            //{
            //    if (!stockRequestCount.ContainsKey(req.StockId))
            //        stockRequestCount.Add(req.StockId, 0);
            //    else
            //        stockRequestCount[req.StockId] += 1;
            //    //db.Stocks.Find(req.StockId).CurrentPrice *= Convert.ToInt32(Math.Ceiling(1.001));
            //}

            //foreach(var req in stockRequestCount.Keys)
            //{
            //    var increase = 1.001 * stockRequestCount[req] / requests.Count();
            //    //db.Stocks.Find(req).CurrentPrice *= Convert.ToInt32(Math.Ceiling(
            //    //    1.0 + Convert.ToDouble(stockRequestCount[req]) / db.OpenRequests.Count()));
            //    db.Stocks.Find(req).CurrentPrice *= Convert.ToInt32(increase);
            //}
            //await db.SaveChangesAsync(stoppingToken);
        }
    }
}
