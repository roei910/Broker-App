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

        public static async void RemoveShare(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<ApiContext>();
            await Task.Delay(5000);
            db.Stocks.Find(1).Amount -= 100;
            db.SaveChanges();
        }

        public static void InitializeData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<ApiContext>();
            InitializeTraders(db);
            InitializeStocks(db);
            db.SaveChanges();
        }

        private static void InitializeTraders(ApiContext db)
        {
            db.Traders.Add(new Trader
            {
                Id = 1,
                Name = "Amit",
                Money = 10000000
            });

            db.Traders.Add(new Trader
            {
                Id = 2,
                Name = "Tal",
                Money = 5000000
            });
            db.Traders.Add(new Trader
            {
                Id = 3,
                Name = "Ran",
                Money = 1000000
            });
            db.Traders.Add(new Trader
            {
                Id = 4,
                Name = "Uri",
                Money = 9000
            });
            db.Traders.Add(new Trader
            {
                Id = 5,
                Name = "Dana",
                Money = 4500000
            });
            db.Traders.Add(new Trader
            {
                Id = 6,
                Name = "Rona",
                Money = 2000000
            });
            db.Traders.Add(new Trader
            {
                Id = 7,
                Name = "Vered",
                Money = 7000000
            });
            db.Traders.Add(new Trader
            {
                Id = 8,
                Name = "Naftali",
                Money = 1000050
            });
            db.Traders.Add(new Trader
            {
                Id = 9,
                Name = "Ronen",
                Money = 8000000
            });
            db.Traders.Add(new Trader
            {
                Id = 10,
                Name = "Samuel",
                Money = 5000000
            });
            db.Traders.Add(new Trader
            {
                Id = 11,
                Name = "Avishay",
                Money = 987654321
            });
            db.Traders.Add(new Trader
            {
                Id = 12,
                Name = "Rinat",
                Money = 1234567
            });
            db.Traders.Add(new Trader
            {
                Id = 13,
                Name = "Ella",
                Money = 4567890
            });
            db.Traders.Add(new Trader
            {
                Id = 14,
                Name = "Melany",
                Money = 1000000
            });
            db.Traders.Add(new Trader
            {
                Id = 15,
                Name = "Nicco",
                Money = 100000
            });
            db.Traders.Add(new Trader
            {
                Id = 16,
                Name = "Dan",
                Money = 50000
            });
            db.Traders.Add(new Trader
            {
                Id = 17,
                Name = "Eldad",
                Money = 17000
            });
            db.Traders.Add(new Trader
            {
                Id = 18,
                Name = "Hadar",
                Money = 5000000
            });
            db.Traders.Add(new Trader
            {
                Id = 19,
                Name = "Galit",
                Money = 3000000
            });
            db.Traders.Add(new Trader
            {
                Id = 20,
                Name = "Liat",
                Money = 100000
            });
        }

        private static void InitializeStocks(ApiContext db)
        {
            db.Stocks.Add(new Stock
            {
                Id = 1,
                Name = "Apple",
                CurrentPrice = 230,
                Amount = 15000
            });
            db.Stocks.Add(new Stock
            {
                Id = 2,
                Name = "Microsoft",
                CurrentPrice = 330,
                Amount = 10000
            });
            db.Stocks.Add(new Stock
            {
                Id = 3,
                Name = "Tesla",
                CurrentPrice = 1500,
                Amount = 1000
            });
            db.Stocks.Add(new Stock
            {
                Id = 4,
                Name = "Ford",
                CurrentPrice = 33,
                Amount = 100000
            });
            db.Stocks.Add(new Stock
            {
                Id = 5,
                Name = "Gamestop",
                CurrentPrice = 2,
                Amount = 230000
            });
            db.Stocks.Add(new Stock
            {
                Id = 6,
                Name = "Meta",
                CurrentPrice = 100,
                Amount = 35000
            });
            db.Stocks.Add(new Stock
            {
                Id = 7,
                Name = "Uniliver",
                CurrentPrice = 75,
                Amount = 17000
            });
            db.Stocks.Add(new Stock
            {
                Id = 8,
                Name = "BMW",
                CurrentPrice = 50,
                Amount = 10000
            });
            db.Stocks.Add(new Stock
            {
                Id = 9,
                Name = "McDonalds",
                CurrentPrice = 132,
                Amount = 45000
            });
            db.Stocks.Add(new Stock
            {
                Id = 10,
                Name = "Burger King",
                CurrentPrice = 99,
                Amount = 30000
            });
            db.Stocks.Add(new Stock
            {
                Id = 11,
                Name = "Samsung",
                CurrentPrice = 67,
                Amount = 12000
            });
            db.Stocks.Add(new Stock
            {
                Id = 12,
                Name = "Xiaomi",
                CurrentPrice = 18,
                Amount = 100000
            });
            db.Stocks.Add(new Stock
            {
                Id = 13,
                Name = "HP",
                CurrentPrice = 830,
                Amount = 1500
            });
            db.Stocks.Add(new Stock
            {
                Id = 14,
                Name = "Dyson",
                CurrentPrice = 980,
                Amount = 160
            });
            db.Stocks.Add(new Stock
            {
                Id = 15,
                Name = "GoldenShare",
                CurrentPrice = 10000000,
                Amount = 1
            });
            db.Stocks.Add(new Stock
            {
                Id = 16,
                Name = "CheapShare",
                CurrentPrice = 1,
                Amount = 100000000
            });
        }
    }
}
