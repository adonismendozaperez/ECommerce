using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Classes;

namespace ECommerce.Controllers
{
    [Authorize(Roles ="user")]
    public class WarehousesController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: Warehouses
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouses = db.Warehouses.Where(c=>c.CompanyId == user.CompanyId).Include(w => w.City).Include(w => w.Department);
            return View(warehouses.ToList());
        }

        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouses = new Warehouse { CompanyId = user.CompanyId ,};
            return View(warehouses);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                
                db.Warehouses.Add(warehouse);
                var reponse = DbHelper.SaveChanges(db);
                if (reponse.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, reponse.Message);  
                
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(warehouse.DepartmentId), "CityId", "Name", warehouse.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", warehouse.DepartmentId);
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(warehouse.DepartmentId), "CityId", "Name", warehouse.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", warehouse.DepartmentId);
            return View(warehouse);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WarehouseId,CompanyId,Name,Phone,Address,DepartmentId,CityId")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                
                  db.Entry(warehouse).State = EntityState.Modified;
                  var reponse=  DbHelper.SaveChanges(db);
                if (reponse.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, reponse.Message);

            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(warehouse.DepartmentId), "CityId", "Name", warehouse.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", warehouse.DepartmentId);
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse warehouse = db.Warehouses.Find(id);
            db.Warehouses.Remove(warehouse);
            var reponse= DbHelper.SaveChanges(db);
            if (reponse.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, reponse.Message);
            return View(warehouse);
            

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
