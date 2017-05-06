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
    [Authorize(Roles ="user")]
    public class OrdersController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        [HttpPost]
        public ActionResult AddProduct(AddProductView productview)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var orderTmp = db.OrderDetailTmps.Where(o => o.UserName == User.Identity.Name &&o.ProductId == productview.ProductId).FirstOrDefault();
                var product = db.Products.Find(productview.ProductId);
                if (orderTmp == null)
                {           
                    orderTmp = new OrderDetailTmp
                     {
                    Description = product.Description,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    Quantity = productview.Quantity,
                    TaxRate = product.Tax.Rate,
                    UserName = User.Identity.Name,
                      };
                    db.OrderDetailTmps.Add(orderTmp);
                }
                else
                {
                    orderTmp.Quantity += productview.Quantity;
                    db.Entry(orderTmp).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Create");
            }
           
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId), "ProductId", "Name");
            return PartialView(productview);
        }

        public ActionResult AddProduct()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId,true), "ProductId", "Name");

            return PartialView();
        }

        public ActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderTmp = db.OrderDetailTmps.Where(o => o.UserName == User.Identity.Name && o.ProductId == id).FirstOrDefault();

            if (orderTmp == null)
            {
                return HttpNotFound();
            }
            db.OrderDetailTmps.Remove(orderTmp);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        // GET: Orders
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var orders = db.Orders.Where(o=> o.CompanyId == user.CompanyId).Include(o => o.Customer).Include(o => o.State).OrderBy(o=>o.Date);
            return View(orders.ToPagedList((int)page, 5));
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail order = db.OrderDetails.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName");
            var NewOrder = new NewOrderView
            {
                Date = DateTime.Now,
                Details = db.OrderDetailTmps.Where(tmp => tmp.UserName == User.Identity.Name).ToList(),

            };
            return View(NewOrder);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewOrderView NewOrder)
        {
            if (ModelState.IsValid)
            {
                var reponse = MovementsHelper.NewOrder(NewOrder, User.Identity.Name);
                if (reponse.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, reponse.Message);
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName");
            ViewBag.Details = db.OrderDetailTmps.Where(tmp => tmp.UserName == User.Identity.Name).ToList();
            return View(NewOrder);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "UserName", order.CustomerId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
