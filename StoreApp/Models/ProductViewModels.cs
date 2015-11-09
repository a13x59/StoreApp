using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class ProductEditViewModel
    {
        public Product product { get; set; }

        public List<Detail> details { get; set; }
    }
}