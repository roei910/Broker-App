using System.ComponentModel.DataAnnotations.Schema;

namespace BrokerAppAPI.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int CurrentPrice { get; set; }
        public int Amount { get; set; }

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
    }
}
