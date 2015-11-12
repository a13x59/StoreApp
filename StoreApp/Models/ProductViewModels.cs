using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class ProductEditViewModel
    {
        public Product product { get; set; }

        public List<ProductDetailDto> details { get; set; }
    }

    public class ProductDetailDto
    {
        public int id { set; get; }

        public string name { set; get; }

        public int count { set; get; }
    }

    public class ProductMaterialDto
    {
        public int product_id { set; get; }

        public int material_id { set; get; }

        public int sum { set; get; }
    }
}