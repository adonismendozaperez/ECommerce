using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext():base ("DefaultConnection")
        {

        }
        //Metodo para que no me permita eliminar en cascada
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
          
        }

        public DbSet<ECommerce.Models.Department> Departments { get; set; }

        public DbSet<ECommerce.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Models.User> Users { get; set; }
    }
}