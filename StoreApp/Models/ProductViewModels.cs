using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class ProductEditViewModels
    {
        Product product { get; set; }

        List<Detail> details { get; set; }
    }
}