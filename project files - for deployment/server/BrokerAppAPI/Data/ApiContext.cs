using BrokerAppAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace BrokerAppAPI.Data
{
    public class ApiContext: DbContext
    { 
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<ShareRequest> OpenRequests { get; set; }
        public DbSet<Share> TradersShares { get; set; }
        public DbSet<SharePurchase> TransactionHistory { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public static void InitializeData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<ApiContext>();
            var data = File.ReadAllText("./Data/Data.json");
            if (db == null || data == null)
                return;
            dynamic obj = JsonConvert.DeserializeObject(data);
            if (obj == null)
                return;
            foreach(dynamic trader in obj.traders)
            {
                db.Traders.Add(new Trader
                {
                    Id = trader.id,
                    Name = trader.name,
                    Money = trader.money
                });
            }
            foreach (dynamic stock in obj.shares)
            {
                db.Stocks.Add(new Stock
                {
                    Id = stock.id,
                    Name = stock.name,
                    CurrentPrice = stock.currentPrice,
                    Amount = stock.amount
                });
            }
            db.SaveChanges();
        }
    }
}
