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
    public class MaterialsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Materials
        public ActionResult Index()
        {
            return View(unitOfWork.MaterialsRepository.Get());
        }

        // GET: Materials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "material_id,name,count")] Material material)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.MaterialsRepository.Insert(material);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(material);
        }

        // GET: Materials/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Material material = unitOfWork.MaterialsRepository.GetById(id.Value);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "material_id,name,count")] Material material)
        public ActionResult Edit([Bind(Include = "material_id,name,count")] Material material)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.MaterialsRepository.Update(material);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(material);
        }

        // GET: Materials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = unitOfWork.MaterialsRepository.GetById(id);

            if (material == null)
            {
                return HttpNotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Material detail = unitOfWork.MaterialsRepository.GetById(id);
            unitOfWork.MaterialsRepository.Delete(id);
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
