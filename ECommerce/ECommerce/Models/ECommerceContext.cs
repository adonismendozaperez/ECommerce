using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext():base ("DefaultConnection")
        {

        }

        public DbSet<ECommerce.Models.Department> Departments { get; set; }

        public DbSet<ECommerce.Models.City> Cities { get; set; }
    }
}