using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class ProductDetailModel
    {
        public long ProductId { get; set; }
        public long SizeId { get; set; }
        public string SizeName { get; set; }
        public int Stock { get; set; }
    }
}