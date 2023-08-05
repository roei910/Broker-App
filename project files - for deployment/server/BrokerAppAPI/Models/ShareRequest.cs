using System;
namespace BrokerAppAPI.Models
{
	public class ShareRequest
	{
        public int Id { get; set; }
        public int StockId { get; set; }
        public int TraderId { get; set; }
        public double Offer { get; set; }
        public int Amount { get; set; }
		public bool Purchase { get; set; }
	}
}

