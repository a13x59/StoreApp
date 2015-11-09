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

            ProductEditViewModel model = new ProductEditViewModel()
            {
                product = product,
                details = details
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
            ProductsDetail productsDetail = await db.ProductsDetails.Where(el => el.id == id.Value).FirstOrDefaultAsync();//await db.ProductsDetails.FindAsync(id);
            if (productsDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productsDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productsDetail.product_id);
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
                return RedirectToAction("Index");
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
            ProductsDetail productsDetail = await db.ProductsDetails.FindAsync(id);
            if (productsDetail == null)
            {
                return HttpNotFound();
            }
            return View(productsDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDetailConfirmed(int id)
        {
            ProductsDetail productsDetail = await db.ProductsDetails.FindAsync(id);
            db.ProductsDetails.Remove(productsDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
