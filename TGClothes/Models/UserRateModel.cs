using Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class UserRateModel
    {
        public Account User { get; set; }
        public Rate Rate { get; set; }
    }
}