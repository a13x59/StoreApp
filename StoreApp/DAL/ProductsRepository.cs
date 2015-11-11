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

        public List<ProductMaterialDto> GetProductMaterialsSum(IEnumerable<Material> materials)
        {
            //TODO: move to resources
            var query = "SELECT pd.product_id, d.material_id, SUM(d.count * pd.count) AS 'sum' FROM ProductsDetails pd " +
                        "JOIN Details d " +
                        "ON d.id = pd.detail_id " +
                        "GROUP BY pd.product_id, d.material_id " +
                        "ORDER BY pd.product_id";

            var data = this.context.Database.SqlQuery<ProductMaterialDto>(query);

            return data.ToList();
        }
    }
}