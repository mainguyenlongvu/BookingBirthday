using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public string PriceSortOrder { get; set; }
        public string DateSortOrder { get; set; }
    }
}