using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreApp.Models;
using StoreApp.DAL;
using System.Data.Entity.Infrastructure;

namespace StoreApp.Controllers
{
    public class StatisticsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Statistics
        public ActionResult Index()
        {
            List<StatisticsViewModel> result = new List<StatisticsViewModel>();
            List<ProductMaterialDto> productMaterialsList = unitOfWork.ProductsRepository.GetProductMaterialsSum();
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
                    productId = product.id,
                    productName = product.name,
                    count = 0
                };

                foreach (var item in pmdList)
                {
                    material = materialsList.First(m => m.material_id == item.material_id);
                    productsCount = material.count / item.sum;

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

        public ActionResult GetProductsSum()
        {
            List<StatisticsViewModel> result = new List<StatisticsViewModel>();
            List<ProductMaterialDto> productMaterialsList = unitOfWork.ProductsRepository.GetProductMaterialsSum();
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
                    productId = product.id,
                    productName = product.name,
                    count = 0
                };

                foreach (var item in pmdList)
                {
                    material = materialsList.First(m => m.material_id == item.material_id);
                    productsCount = material.count / item.sum;

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

            return Json(result);
        }

        public ActionResult MakeProduct(int id)
        {
            try
            {
                Product product = unitOfWork.ProductsRepository.GetById(id);

                if (product == null)
                {
                    throw new Exception("Продукт не найден");
                }

                //сколько нужно каждого материала?
                List<ProductMaterialDto> pmDto = unitOfWork.ProductsRepository.GetProductMaterialsSum(id);
                ProductMaterialDto pm;

                int[] mIds = pmDto.Select(m => m.material_id).ToArray();

                if (mIds.Length == 0)
                {
                    throw new Exception("Не заданы материалы для продукта");
                }

                List<Material> productMaterials = unitOfWork.MaterialsRepository.Get(m => mIds.Contains(m.material_id)).ToList();

                foreach (var material in productMaterials)
                {
                    pm = pmDto.First(el => el.material_id == material.material_id);

                    if (pm.sum > material.count)
                    {
                        throw new Exception("Недостаточно материала: " + material.name);
                    }

                    material.count -= pm.sum;//в этот момент данные могли измениться
                    unitOfWork.MaterialsRepository.Update(material);
                }
                unitOfWork.Save();//при отличии rowversion генерируется исключение DbUpdateConcurrencyException

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Json(new { result = "error", message = "Количество материалов на складе изменилось, попробуйте обновить данные" + ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", message = ex.Message });
            }
            return Json(new { result = id });
        }
    }
}