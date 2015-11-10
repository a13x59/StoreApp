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

namespace StoreApp.Controllers
{
    public class ProductsController : Controller
    {
        private StorageDataBaseEntities db = new StorageDataBaseEntities();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
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

            Product product = db.Products.FirstOrDefault(p => p.id == id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.product = product;

            IEnumerable<Detail> details = db.Details;

            int[] productDetailsIds = product.ProductsDetails.Select(pd => pd.detail_id).ToArray();

            if (productDetailsIds.Count() != 0)
            {
                details = db.Details.Where(d => !productDetailsIds.Contains(d.id));
            }

            ViewBag.detail_id = new SelectList(details, "id", "name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddDetail([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productsDetail)
        {
            if (ModelState.IsValid)
            {
                db.ProductsDetails.Add(productsDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = productsDetail.product_id });
                //return RedirectToAction("Index");
            }

            Product product = db.Products.FirstOrDefault(p => p.id == productsDetail.product_id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.product = product;

            //TODO - method
            IEnumerable<Detail> details = db.Details;

            int[] productDetailsIds = product.ProductsDetails.Select(pd => pd.detail_id).ToArray();

            if (productDetailsIds.Count() != 0)
            {
                details = db.Details.Where(d => !productDetailsIds.Contains(d.id));
            }


            ViewBag.detail_id = new SelectList(details, "id", "name", productsDetail.detail_id);
            //ViewBag.product_id = new SelectList(db.Products, "id", "name", productsDetail.product_id);
            return View(productsDetail);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            List<int> details_ids = product.ProductsDetails.Select(pd => pd.detail_id).ToList();

            List<Detail> details = db.Details.Where(d => details_ids.Contains(d.id)).ToList();

            List<ProductDetailDto> productDetails = new List<ProductDetailDto>();

            foreach (var productDetail in product.ProductsDetails)
            {
                productDetails.Add(new ProductDetailDto(productDetail, details));
            }

            ProductEditViewModel model = new ProductEditViewModel()
            {
                product = product,
                details = productDetails
            };

            return View(model);
            //return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<ActionResult> EditDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductsDetail productsDetail = await db.ProductsDetails.FirstOrDefaultAsync(el => el.id == id.Value);//await db.ProductsDetails.FindAsync(id);

            if (productsDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.product = db.Products.First(el => el.id == productsDetail.product_id);
            ViewBag.detail = db.Details.First(el => el.id == productsDetail.detail_id);

            return View(productsDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDetail([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productsDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = productsDetail.product_id });
            }
            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productsDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productsDetail.product_id);
            return View(productsDetail);
        }

        public async Task<ActionResult> DeleteDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsDetail productsDetail = await db.ProductsDetails.FirstOrDefaultAsync(pd => pd.id == id.Value);
            if (productsDetail == null)
            {
                return HttpNotFound();
            }
            return View(productsDetail);
        }

        [HttpPost, ActionName("DeleteDetail")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDetailConfirmed(int id)
        {
            ProductsDetail productsDetail = await db.ProductsDetails.FirstOrDefaultAsync(pd => pd.id == id);
            db.ProductsDetails.Remove(productsDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = productsDetail.product_id });
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
