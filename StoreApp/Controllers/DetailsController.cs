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
    public class DetailsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Details
        public ActionResult Index()
        {
            return View(unitOfWork.DetailsRepository.Get());
        }

        // GET: Details/Create
        public ActionResult Create()
        {
            ViewBag.material_id = new SelectList(unitOfWork.MaterialsRepository.Get(), "material_id", "name");
            return View();
        }

        // POST: Details/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,material_id,count")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.DetailsRepository.Insert(detail);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }

            ViewBag.material_id = new SelectList(unitOfWork.MaterialsRepository.Get(), "material_id", "name", detail.material_id);
            return View(detail);
        }

        // GET: Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = unitOfWork.DetailsRepository.GetById(id);//db.Details.FindAsync(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.material_id = new SelectList(unitOfWork.MaterialsRepository.Get(), "material_id", "name", detail.material_id);
            return View(detail);
        }

        // POST: Details/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,material_id,count")] Detail detail)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.DetailsRepository.Update(detail);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            ViewBag.material_id = new SelectList(unitOfWork.MaterialsRepository.Get(), "material_id", "name", detail.material_id);
            return View(detail);
        }

        // GET: Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail detail = unitOfWork.DetailsRepository.GetById(id);

            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        // POST: Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.DetailsRepository.Delete(id);
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
