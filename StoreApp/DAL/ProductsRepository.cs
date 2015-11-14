using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.DAL
{
    public class ProductsRepository : GenericRepository<Product>
    {
        public ProductsRepository(StorageDataBaseEntities context) : base(context)
        {

        }

        /// <summary>
        /// мой метод вычисления необходимого кол-ва материалов для изделия
        /// </summary>
        public List<ProductMaterialDto> GetProductMaterialsSum(int? id = null)
        {
            string query;

            if (id.HasValue)
            {
                query = String.Format(StoreAppSqlResources.ProductMaterialsQuery, "WHERE pd.product_id = @p0");
                var data = this.context.Database.SqlQuery<ProductMaterialDto>(query, id.Value);

                return data.ToList();
            }
            else
            {
                query = String.Format(StoreAppSqlResources.ProductMaterialsQuery, String.Empty);
                var data = this.context.Database.SqlQuery<ProductMaterialDto>(query);

                return data.ToList();
            }
        }
    }
}