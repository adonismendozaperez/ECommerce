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
using PagedList;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: Companies
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var companies = db.Companies.Include(c => c.City).Include(c => c.Department).OrderBy(c => c.Name);
            return View(companies.ToPagedList((int)page, 5));
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(0),"CityId","Name");
            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),"DepartmentId","Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
             
                
                    db.Companies.Add(company);
                    db.SaveChanges();

                    if (company.LogoFile != null)
                    {
                        
                        var folder = "~/Content/Logos";
                        var file = string.Format("{0}.jpg", company.CompanyId);
                        var reponses = FilesHelper.UploadPhoto(company.LogoFile, folder,file);
                        if (reponses)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            company.Logo = pic;
                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        
                    }
                    
                    return RedirectToAction("Index");
                
            }

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(company.DepartmentId),"CityId", "Name", company.CityId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),"DepartmentId","Name",company.DepartmentId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = db.Companies.Find(id);

            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(company.DepartmentId),"CityId","Name", company.CityId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(), "DepartmentId","Name", company.DepartmentId);
            return View(company);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (company.LogoFile != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/Logos";
                        var file = string.Format("{0}.jpg", company.CompanyId);
                        var  reponse = FilesHelper.UploadPhoto(company.LogoFile, folder,file);
                        if (reponse)
                        {
                             pic = string.Format("{0}/{1}", folder, file);
                            company.Logo = pic;
                           
                        }
                       
                    }
                    db.Entry(company).State = EntityState.Modified;
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
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId),"CityId", "Name", company.CityId);

            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId","Name",company.DepartmentId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
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
