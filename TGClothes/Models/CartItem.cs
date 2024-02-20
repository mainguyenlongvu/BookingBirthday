using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { get; set; }
        public Size Size { get; set; }
        public ProductSize ProductSize { get; set; }
        public int Quantity { get; set; }
    }
}