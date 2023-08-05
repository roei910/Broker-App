using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BrokerAppAPI.Models
{
    //[Keyless]
    public class Trader
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Money { get; set; }
    }
}
