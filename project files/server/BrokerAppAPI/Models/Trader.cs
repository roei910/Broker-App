using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrokerAppAPI.Models
{
    //[Keyless]
    public class Trader
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Money { get; set; }
        
        //public Trader() { }

        //public Trader(int id, string name, int money, Share share)
        //{
        //    Id = id;
        //    Name = name;
        //    Money = money;
        //    TraderShares = new List<Share> { share };
        //    //this.LastDeals = new List<Deal>();
        //    //this.OpenDeals = new List<Deal>();
        //}
        //public bool RemoveOpenDeal(Deal deal)
        //{
        //    return OpenDeals.Remove(deal);
        //}
        //public void AddOpenDeal(Deal deal)
        //{
        //    OpenDeals.Add(deal);
        //}
        //public void AddDeal(Deal deal)
        //{
        //    LastDeals.Insert(0, deal);
        //}
        //public void AddShare(int shareId, int amount)
        //{
        //    bool foundShare = false;
        //    foreach (Share share in TraderShares)
        //    {
        //        if (share.StockId == shareId)
        //        {
        //            share.Amount += amount;
        //            foundShare = true;
        //        }
        //    }
        //    if (!foundShare)
        //        TraderShares.Add(
        //            new Share
        //            {
        //                Amount = amount,
        //                StockId = shareId
        //            });

        //}
    }
}
