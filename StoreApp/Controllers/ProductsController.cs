using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreApp.Models;
using StoreApp.DAL;

namespace StoreApp.Controllers
{
    public class ProductsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Products
        public ActionResult Index()
        {
            //return View(await db.Products.ToListAsync());
            return View(unitOfWork.ProductsRepository.Get());
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductsRepository.Insert(product);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult AddDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = unitOfWork.ProductsRepository.GetById(id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.product = product;

            _addProductDetailsInfo(product, null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetail([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductDetailsRepository.Insert(productDetail);
                unitOfWork.Save();

                return RedirectToAction("Edit", new { id = productDetail.product_id });
            }

            Product product = unitOfWork.ProductsRepository.GetById(productDetail.product_id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.product = product;

            _addProductDetailsInfo(product, productDetail, true);

            return View(productDetail);
        }

        private void _addProductDetailsInfo(Product product, ProductsDetail productDetail, bool setDetailId = false)
        {
            int? detailId = null;

            if (productDetail != null)
                detailId = productDetail.detail_id;

            int[] productDetailsIds = product.ProductsDetails.Select(pd => pd.detail_id).ToArray();
            IEnumerable<Detail> details = unitOfWork.DetailsRepository.Get(d => !productDetailsIds.Contains(d.id));

            ViewBag.detail_id = new SelectList(details, "id", "name", detailId);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = unitOfWork.ProductsRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            int[] details_ids = product.ProductsDetails.Select(pd => pd.detail_id).ToArray();
            List<Detail> details = unitOfWork.DetailsRepository.Get(d => details_ids.Contains(d.id)).ToList();
            List<ProductDetailDto> productDetailsDto = unitOfWork.ProductDetailsRepository.GetProductDetailsDto(product, details);

            ProductEditViewModel model = new ProductEditViewModel()
            {
                product = product,
                details = productDetailsDto
            };

            return View(model);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductsRepository.Update(product);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult EditDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductsDetail productDetail = unitOfWork.ProductDetailsRepository.GetById(id);

            if (productDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.product = unitOfWork.ProductsRepository.GetById(productDetail.product_id);
            ViewBag.detail = unitOfWork.DetailsRepository.GetById(productDetail.detail_id);

            return View(productDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductDetailsRepository.Update(productDetail);
                unitOfWork.Save();

                return RedirectToAction("Edit", new { id = productDetail.product_id });
            }
            ViewBag.detail_id = new SelectList(unitOfWork.DetailsRepository.Get(), "id", "name", productDetail.detail_id);
            ViewBag.product_id = new SelectList(unitOfWork.ProductsRepository.Get(), "id", "name", productDetail.product_id);
            return View(productDetail);
        }

        public ActionResult DeleteDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductsDetail productDetail = unitOfWork.ProductDetailsRepository.GetById(id);

            if (productDetail == null)
            {
                return HttpNotFound();
            }

            return View(productDetail);
        }

        [HttpPost, ActionName("DeleteDetail")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDetailConfirmed(int id)
        {
            ProductsDetail productDetail = unitOfWork.ProductDetailsRepository.GetById(id);
            unitOfWork.ProductDetailsRepository.Delete(productDetail);
            unitOfWork.Save();

            return RedirectToAction("Edit", new { id = productDetail.product_id });
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = unitOfWork.ProductsRepository.GetById(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = unitOfWork.ProductsRepository.GetById(id);
            unitOfWork.ProductsRepository.Delete(product);
            unitOfWork.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
