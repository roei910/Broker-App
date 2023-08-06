namespace BrokerAppAPI.Models
{
    public class SharePurchase
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int TraderId { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public bool Purchase { get; set; }
    }
}
