using System;
using System.Collections.Generic;
using System.Text;

namespace Chnl.Eng.Tech.Assmt.BL.Model
{
    public interface IProductBL 
    {
        List<Product> GetProductsFromOrdersByStatus(string status);

        List<Product> GetTopFive(string status);
    }
}
