using Chnl.Eng.Tech.Assmt.BL.BL;
using Chnl.Eng.Tech.Assmt.BL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Chnl.Eng.Tech.Assmt.UnitTest
{
    public class ProductUnitTest
    {
        
        [Fact]
        public void GetTopFive()
        {
            string APIurl = "https://api-dev.channelengine.net/api/v2/";
            string APIkey = "541b989ef78ccb1bad630ea5b85c6ebff9ca3322";
            List<Product> productsDummy = new List<Product>()
            {
                new Product() { ProductName ="T-shirt met lange mouw BASIC petrol: S",Gtin="8719351029609",Quantity=4,MerchantProductNo = "001201-S"},
                new Product() { ProductName ="T-shirt met lange mouw BASIC petrol: M",Gtin="8719351029609",Quantity=4,MerchantProductNo = "001201-M"},
                new Product() { ProductName ="T-shirt met lange mouw BASIC petrol: XL",Gtin="8719351029609",Quantity=1,MerchantProductNo = "001201-XL"},
                new Product() { ProductName ="T-shirt met lange mouw BASIC petrol: L",Gtin="8719351029609",Quantity=1,MerchantProductNo = "001201-L"},

            };
            ProductBL productBO = new ProductBL(APIurl, APIkey);
            List<Product> lstProduct = productBO.GetTopFive("IN_PROGRESS");
            var object1Json = JsonConvert.SerializeObject(productsDummy);
            var object2Json = JsonConvert.SerializeObject(lstProduct);

            Assert.Equal(object1Json, object2Json);
        }


        [Fact]
        public void SetStockOfProduct()
        {
            string APIurl = "https://api-dev.channelengine.net/api/v2/";
            string APIkey = "541b989ef78ccb1bad630ea5b85c6ebff9ca3322";
            List<Offer> offersDummy = new List<Offer>()
                { new Offer() { MerchantProductNo = Console.ReadLine(), Stock = 25 } };
            ProductBL productBO = new ProductBL(APIurl, APIkey);
            var message = productBO.SetStockOfProduct(offersDummy);
            string expectedmsg = "Updates processed without warnings";

            Assert.Equal(message, expectedmsg);
        }
    }
}
