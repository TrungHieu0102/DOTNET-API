using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
#nullable disable
namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string  AppUserId { get; set; }
        public int StockId { get; set; }
        //Navigate prop
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}