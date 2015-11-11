using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreApp.Models;
using StoreApp.DAL;

namespace StoreApp.Controllers
{
    public class StatisticsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Statistics
        public ActionResult Index()
        {
            List<StatisticsViewModel> result = new List<StatisticsViewModel>();
            List<ProductMaterialDto> productMaterialsList = unitOfWork.ProductsRepository.GetProductMaterialsSum(unitOfWork.MaterialsRepository.Get());
            List<ProductMaterialDto> pmdList = new List<ProductMaterialDto>();
            Material material;
            var materialsList = unitOfWork.MaterialsRepository.Get();
            var allProductsList = unitOfWork.ProductsRepository.Get();
            int[] productIds = new int[] { };
            int? minCount, productsCount = 0;

            foreach (var product in allProductsList)
            {
                productIds = product.ProductsDetails.Select(p => p.product_id).ToArray();
                pmdList = productMaterialsList.Where(it => productIds.Contains(it.product_id)).ToList();
                minCount = null;

                StatisticsViewModel swm = new StatisticsViewModel()
                {
                    productName = product.name,
                    count = 0
                };

                foreach (var item in pmdList)
                {
                    material = materialsList.First(m => m.material_id == item.material_id);
                    productsCount = (int)((double)material.count / item.sum);

                    if (productsCount == 0)
                    {
                        minCount = 0;
                        break;
                    }

                    if (!minCount.HasValue || productsCount < minCount)
                        minCount = productsCount;
                }

                swm.count = minCount.Value;
                result.Add(swm);
            }

            return View(result);
        }
    }
}