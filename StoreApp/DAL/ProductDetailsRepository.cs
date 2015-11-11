using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.DAL
{
    public class ProductDetailsRepository : GenericRepository<ProductsDetail>
    {
        public ProductDetailsRepository(StorageDataBaseEntities context) : base(context)
        {

        }

        public override ProductsDetail GetById(object id)
        {
            return dbSet.FirstOrDefault(pd => pd.id == (int)id);
        }

        public List<ProductDetailDto> GetProductDetailsDto(Product product, IEnumerable<Detail> details)
        {
            var productDetailsDto = new List<ProductDetailDto>();

            foreach (var productDetail in product.ProductsDetails)
            {
                Detail detail = details.First(d => d.id == productDetail.detail_id);

                productDetailsDto.Add(new ProductDetailDto() {
                    id = productDetail.id,
                    name = detail == null ? string.Empty : detail.name,
                    count = productDetail.count
                });
            }

            return productDetailsDto;
        }

    }
}