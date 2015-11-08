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
            ProductsDetail productsDetail = await db.ProductsDetails.FindAsync(id);
            if (productsDetail == null)
            {
                return HttpNotFound();
            }
            return View(productsDetail);
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
        public async Task<ActionResult> Create([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productsDetail)
        {
            if (ModelState.IsValid)
            {
                db.ProductsDetails.Add(productsDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productsDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productsDetail.product_id);
            return View(productsDetail);
        }

        // GET: ProductsDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.detail_id = new SelectList(db.Details, "id", "name", productsDetail.detail_id);
            ViewBag.product_id = new SelectList(db.Products, "id", "name", productsDetail.product_id);
            return View(productsDetail);
        }

        // POST: ProductsDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,product_id,detail_id,count")] ProductsDetail productsDetail)
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

        // GET: ProductsDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: ProductsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductsDetail productsDetail = await db.ProductsDetails.FindAsync(id);
            db.ProductsDetails.Remove(productsDetail);
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
