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
    public class ProductsDetailsController : Controller
    {
        private StorageDataBaseEntities db = new StorageDataBaseEntities();

        // GET: ProductsDetails
        public async Task<ActionResult> Index()
        {
            var productsDetails = db.ProductsDetails.Include(p => p.Detail).Include(p => p.Product);
            return View(await productsDetails.ToListAsync());
        }

        // GET: ProductsDetails/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductsDetail productDetail = await db.ProductsDetails.FindAsync(id);

            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // GET: ProductsDetails/Create
        public ActionResult Create()
        {
            ViewBag.detail_id = new SelectList(db.Details, "id", "name");
            ViewBag.product_id = new SelectList(db.Products, "id", "name");
            return View();
        }

        // POST: ProductsDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.ProductsDetails.Add(productDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productDetail.product_id);
            return View(productDetail);
        }

        // GET: ProductsDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsDetail productDetail = await db.ProductsDetails.Where(el => el.id == id.Value).FirstOrDefaultAsync();//await db.ProductsDetails.FindAsync(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productDetail.product_id);

            return View(productDetail);
        }

        // POST: ProductsDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productDetail.product_id);
            return View(productDetail);
        }

        // GET: ProductsDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsDetail productDetail = await db.ProductsDetails.FindAsync(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // POST: ProductsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductsDetail productDetail = await db.ProductsDetails.FindAsync(id);
            db.ProductsDetails.Remove(productDetail);
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
