using Chnl.Eng.Tech.Assmt.BL.BL;
using Chnl.Eng.Tech.Assmt.BL.Model;
using Chnl.Eng.Tech.Assmt.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Chnl.Eng.Tech.Assmt.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _apikey = "541b989ef78ccb1bad630ea5b85c6ebff9ca3322";
        private readonly string _url = "https://api-dev.channelengine.net/api/v2/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            return View(GetProductsFromOrdersByStatusAsync());
        }

        public IActionResult SetStockOfProduct(string id)
        {
            List<Offer> offers = new List<Offer>()
                { new Offer() { MerchantProductNo =id, Stock = 25 } };
            ProductBL productBO = new ProductBL(_url, _apikey);
            ViewData["Message"] = productBO.SetStockOfProduct(offers);
            return View("Index", GetProductsFromOrdersByStatusAsync());

        }

        public List<Product> GetProductsFromOrdersByStatusAsync()
        {
            ProductBL productBO = new ProductBL(_url, _apikey);
            List<Product> lstProduct = productBO.GetTopFive("IN_PROGRESS");
            return lstProduct;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
