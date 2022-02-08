using Chnl.Eng.Tech.Assmt.BL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;


namespace Chnl.Eng.Tech.Assmt.BL.BL
{
    public class ProductBL : IProductBL
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private string _apikey = "541b989ef78ccb1bad630ea5b85c6ebff9ca3322";
        private string _url = "https://api-dev.channelengine.net/api/v2/";

        public ProductBL(string apiUrl, string apikey)
        {
            _apikey = apikey;
            _url = apiUrl;
        }
        /// <summary>
        /// Fetch all orders with status from the  Channel Engien API
        /// </summary>
        /// <param name="status">IN_PROGRESS</param>
        /// <returns></returns>
        public List<Product> GetProductsFromOrdersByStatus(string status)
        {
            List<Product> lstProducts = new List<Product>();
            try
            {
                Dictionary<string, string> query = new Dictionary<string, string>();
                query.Add("statuses", status);

                dynamic response = GetOperationResult<dynamic>("GET", "orders", query);


                foreach (var order in response.Content)
                {
                    foreach (var line in order.Lines)
                    {
                        string merchantProductNo = line.MerchantProductNo;
                        Product product = lstProducts.Where(x => x.MerchantProductNo.Equals(merchantProductNo)).FirstOrDefault();
                        if (product != null)
                        {
                            product.Quantity += (int)line.Quantity;
                        }
                        else
                        {
                            product = new Product
                            {
                                ProductName = line.Description, //Same as the product name
                                Quantity = (int)line.Quantity,
                                Gtin = line.Gtin,
                                MerchantProductNo = line.MerchantProductNo,

                            };
                            lstProducts.Add(product);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstProducts;
        }

        /// <summary>
        /// Get the top five froducts order by the total quantity sold in descending order
        /// </summary>
        /// <param name="status">IN_PROGRESS</param>
        /// <returns></returns>
        public List<Product> GetTopFive(string status)
        {
            return GetProductsFromOrdersByStatus(status).OrderByDescending(p => p.Quantity).Take(5).ToList();
        }

        /// <summary>
        /// set the stock of a product to n.
        /// </summary>
        /// <param name="offers"></param>
        /// <returns></returns>
        public string SetStockOfProduct(List<Offer> offers)
        {
            string result;
            try
            {
                Dictionary<string, string> query = new Dictionary<string, string>();
                dynamic response = GetOperationResult<dynamic>("PUT", "offer", query, offers);

                result = response.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string ParamsToString(Dictionary<string, string> urlParams)
        {
            using (HttpContent content = new FormUrlEncodedContent(urlParams))
                return content.ReadAsStringAsync().GetAwaiter().GetResult();
        }


        private T GetOperationResult<T>(string verb, string path, Dictionary<string, string> urlParams, object jsonData = null)
        {

            urlParams.Add("apikey", _apikey);
            string query = ParamsToString(urlParams);
            HttpResponseMessage response = default(HttpResponseMessage);

            switch (verb)
            {
                case "GET":
                    response = _httpClient.GetAsync(_url + path + "?" + query, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
                    break;
                case "PUT":
                    response = _httpClient.PutAsync(_url + path + "?" + query, new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.Default, "application/json")).GetAwaiter().GetResult();
                    break;
            }

            response.EnsureSuccessStatusCode();
            string responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<T>(responseText);

        }
    }
}
