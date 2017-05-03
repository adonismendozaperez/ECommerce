using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class GenericController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

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