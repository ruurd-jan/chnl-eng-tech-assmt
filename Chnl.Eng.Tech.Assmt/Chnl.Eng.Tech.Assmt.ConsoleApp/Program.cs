using Chnl.Eng.Tech.Assmt.BL.BL;
using Chnl.Eng.Tech.Assmt.BL.Model;
using ConsoleTables;
using System;
using System.Collections.Generic;

namespace Chnl.Eng.Tech.Assmt.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            string APIurl =  "https://api-dev.channelengine.net/api/v2/";
            string APIkey = "541b989ef78ccb1bad630ea5b85c6ebff9ca3322";

            try
            {
                ProductBL productBO = new ProductBL(APIurl, APIkey);
                List<Product> lstProduct = productBO.GetTopFive("IN_PROGRESS");

                Console.WriteLine("TOP 5 PRODUCTS");
                ConsoleTable
                   .From<Product>(lstProduct)
                   .Configure(o =>
                   {
                       o.NumberAlignment = Alignment.Right;
                       o.EnableCount = true;
                     
                   })
                   .Write(Format.Alternative);

                Console.WriteLine("Write the MerchantProductNo to set the stock of that product to 25:");
                List<Offer> offers = new List<Offer>()
                { new Offer() { MerchantProductNo = Console.ReadLine(), Stock = 25 } };
                Console.WriteLine(productBO.SetStockOfProduct(offers));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
