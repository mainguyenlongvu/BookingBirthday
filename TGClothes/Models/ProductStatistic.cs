using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class ProductStatistic
    {
        public Product Product { get; set; }
        public int ProductSold { get; set; }
        public decimal Price { get; set; }
    }
}