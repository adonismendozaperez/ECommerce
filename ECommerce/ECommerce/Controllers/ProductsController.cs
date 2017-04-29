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
    public class ProductsController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Tax)
                .Where(p=>p.CompanyId == user.CompanyId);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(user.CompanyId), "CategoryId", "Name");
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(user.CompanyId), "TaxId", "Name");

            var product = new Product {
                CompanyId = user.CompanyId,
            };
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    if (product.ImageFiler != null)
                    {

                        var folder = "~/Content/Products";
                        var file = string.Format("{0}.jpg", product.ProductId);
                        var reponse = FilesHelper.UploadPhoto(product.ImageFiler, folder, file);
                        if (reponse)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            product.Image = pic;
                            db.Entry(product).State = EntityState.Modified;
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

            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(user.CompanyId), "CategoryId", "Name", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(user.CompanyId), "TaxId", "Name", product.TaxId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(product.CompanyId), "CategoryId", "Name", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(product.CompanyId), "TaxId", "Name", product.TaxId);
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFiler != null)
                    {
                        var pic = string.Empty;
                        var folder = "~/Content/Logos";
                        var file = string.Format("{0}.jpg", product.ProductId);
                        var reponse = FilesHelper.UploadPhoto(product.ImageFiler, folder, file);
                        if (reponse)
                        {
                            pic = string.Format("{0}/{1}", folder, file);
                            product.Image = pic;

                        }

                    }
                
                    db.Entry(product).State = EntityState.Modified;
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
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(product.CompanyId), "CategoryId", "Name", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(product.CompanyId), "TaxId", "Name", product.TaxId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
