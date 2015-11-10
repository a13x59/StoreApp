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

        public ProductDetailDto(ProductsDetail productDetail, List<Detail> details)
        {
            Detail detail = details.FirstOrDefault(d => d.id == productDetail.detail_id);

            id = productDetail.id;
            name = detail == null ? String.Empty : detail.name;
            count = productDetail.count;
        }
    }
}