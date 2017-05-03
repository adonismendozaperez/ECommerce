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
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( User user)
        {
            if (ModelState.IsValid)
            {
                
                    db.Users.Add(user);
                   var reponse =  DbHelper.SaveChanges(db);
                    if (reponse.Succeeded)
                    {
                        UsersHelper.CreateUserASP(user.UserName, "User");
                        
                    }
                    ModelState.AddModelError(string.Empty, reponse.Message);
                    

                    if (user.PhotoFile != null)
                    {

                        var folder = "~/Content/Photos";
                        var file = string.Format("{0}.jpg", user.UserId);
                        var reponses = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (reponses)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            user.Photo = pic;
                            db.Entry(user).State = EntityState.Modified;
                            DbHelper.SaveChanges(db);
                            if (reponse.Succeeded)
                            {
                                return RedirectToAction("Index");
                            }

                         }
                            ModelState.AddModelError(string.Empty, reponse.Message);
                        }
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(user.DepartmentId), "CityId", "Name", user.CityId);
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
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(user.DepartmentId), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,FirstName,LastName,Phone,Address,Photo,DepartmentId,CityId,CompanyId")] User user)
        {
            if (ModelState.IsValid)
            {
              
                    if (user.PhotoFile != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/Photos";
                        var file = string.Format("{0}.jpg", user.UserId);
                        var reponses = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                        if (reponses)
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
                     var reponse = DbHelper.SaveChanges(db);
                     if (reponse.Succeeded)
                         {
                            return RedirectToAction("Index");
                         }
                ModelState.AddModelError(string.Empty, reponse.Message);
            }
            
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(user.DepartmentId), "CityId", "Name", user.CityId);
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

            var reponse= DbHelper.SaveChanges(db);
            if (reponse.Succeeded)
            {
                UsersHelper.DeleteUser(user.UserName);
                return RedirectToAction("Index");
            }
            return View(user);   
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
