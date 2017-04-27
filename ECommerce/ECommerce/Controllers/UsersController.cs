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
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Company).Include(u => u.Department);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name");
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    UsersHelper.CreateUserASP(user.UserName, "User");

                    if (user.PhotoFile != null)
                    {

                        var folder = "~/Content/Photos";
                        var file = string.Format("{0}.jpg", user.UserId);
                        var reponse = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (reponse)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            user.Photo = pic;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same value");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,FirstName,LastName,Phone,Address,Photo,DepartmentId,CityId,CompanyId")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (user.PhotoFile != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/Photos";
                        var file = string.Format("{0}.jpg", user.UserId);
                        var reponse = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (reponse)
                        {
                            pic = string.Format("{0}/{1}", folder, file);
                            user.Photo = pic;
                        }

                    }
                    var db2 = new ECommerceContext();
                    var currentUser = db2.Users.Find(user.UserId);
                    if(currentUser.UserName != user.UserName)
                    {
                        UsersHelper.UptadeUserName(currentUser.UserName, user.UserName);
                    }
                    db2.Dispose();
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                     ex.InnerException.InnerException != null &&
                     ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same value");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
//ponerle Try cath, para no dejar eliminar usuarios con dependencias
            db.SaveChanges();
            UsersHelper.DeleteUser(user.UserName);
            return RedirectToAction("Index");
        }

        //Metodo para obtener las ciudades relacionadas con los departments
        public JsonResult GetCities(int DepartmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(x => x.DepartmentId == DepartmentId);
            return Json(cities);

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
