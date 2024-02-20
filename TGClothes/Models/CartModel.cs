using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class CartModel
    {
        public long ProductId { get; set; }
        public long SizeId { get; set; }
        public int Quantity { get; set; }
    }
}