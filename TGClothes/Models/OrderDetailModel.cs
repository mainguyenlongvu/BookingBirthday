using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class OrderDetailModel
    {
        public Product Product { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public Order Order { get; set; }
        public Size Size { get; set; }
    }
}