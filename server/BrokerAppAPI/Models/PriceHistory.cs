using System;
namespace BrokerAppAPI.Models
{
	public class PriceHistory
	{
        public int Id { get; set; }
		public List<int> Lst { get; set; } = new List<int>();
	}
}

