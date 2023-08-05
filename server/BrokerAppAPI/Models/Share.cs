using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrokerAppAPI.Models
{
    public class Share
	{
        public int Id { get; set; }
        public int TraderId { get; set; }
        public int StockId { get; set; }
        public int Amount { get; set; }
        public int TotalPrice { get; set; }
    }
}