using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class ProductGalleryModel
    {
        public Product Product { get; set; }
        public Gallery Gallery { get; set; }
    }
}