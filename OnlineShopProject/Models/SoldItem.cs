using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopProject.Models
{
    public class SoldItem
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public int ItemId { get; set; }
        public int ItemCode { get; set; }   
    }
}
