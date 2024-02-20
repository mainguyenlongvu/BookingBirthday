using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class ProvinceData
    {
        public string Province { get; set; }
        public string[] Districts { get; set; }
        public string[] Wards { get; set; }
    }
}