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
            //TODO: move to resources
            var query = "SELECT pd.product_id, d.material_id, SUM(d.count * pd.count) AS 'sum' FROM ProductsDetails pd " +
                        "JOIN Details d " +
                        "ON d.id = pd.detail_id " +
                        "{0} " + 
                        "GROUP BY pd.product_id, d.material_id " +
                        "ORDER BY pd.product_id";

            if (id.HasValue)
            {
                query = String.Format(query, "WHERE pd.product_id = @p0");
                var data = this.context.Database.SqlQuery<ProductMaterialDto>(query, id.Value);

                return data.ToList();
            }
            else
            {
                query = String.Format(query, String.Empty);
                var data = this.context.Database.SqlQuery<ProductMaterialDto>(query);

                return data.ToList();
            }
        }
    }
}