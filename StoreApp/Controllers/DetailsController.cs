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
    public class DetailsController : Controller
    {
        private StorageDataBaseEntities db = new StorageDataBaseEntities();

        // GET: Details
        public async Task<ActionResult> Index()
        {
            var details = db.Details.Include(d => d.Material);
            return View(await details.ToListAsync());
        }

        // GET: Details/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = await db.Details.FindAsync(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        // GET: Details/Create
        public ActionResult Create()
        {
            ViewBag.material_id = new SelectList(db.Materials, "material_id", "name");
            return View();
        }

        // POST: Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name,material_id,count")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                db.Details.Add(detail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.material_id = new SelectList(db.Materials, "material_id", "name", detail.material_id);
            return View(detail);
        }

        // GET: Details/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = await db.Details.FindAsync(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.material_id = new SelectList(db.Materials, "material_id", "name", detail.material_id);
            return View(detail);
        }

        // POST: Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name,material_id,count")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.material_id = new SelectList(db.Materials, "material_id", "name", detail.material_id);
            return View(detail);
        }

        // GET: Details/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = await db.Details.FindAsync(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        // POST: Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Detail detail = await db.Details.FindAsync(id);
            db.Details.Remove(detail);
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
