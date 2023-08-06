using System.ComponentModel.DataAnnotations.Schema;

namespace BrokerAppAPI.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double CurrentPrice { get; set; }
        public int Amount { get; set; }
        public int CountPurchase { get; set; } = 0;
        public int CountSale { get; set; } = 0;
    }
}
