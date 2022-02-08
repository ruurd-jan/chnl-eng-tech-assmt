using System;
using System.Collections.Generic;
using System.Text;

namespace Chnl.Eng.Tech.Assmt.BL.Model
{
    public class Product
    {
        public string MerchantProductNo { get; set; }
        public string ProductName { get; set; }
        public string Gtin { get; set; }
        public int Quantity { get; set; }
    }
}
