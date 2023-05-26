using OnlineShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopProject.MyViewModels
{
    public class SoldItemViewModel
    {
        public int Id { get; set; } 

        public int ItemCode { get;set; }
        public string? Name { get; set; } 
        public string? CustomerEmail { get; set; }

        public int itemId { private get; init; }
        public SoldItem ToSoldItemDBModel()
        {
            return new SoldItem()
            {
                Id = this.Id,
                CustomerEmail = this.CustomerEmail,
                ItemCode = this.ItemCode,
                ItemId = itemId
            };
        }
    }
}
