using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        [Display(Name ="company")]
        [Index("companyName",IsUnique =true)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }    
      
        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public int DepartmentId { get; set; }


        [Required]
        [Range(1, double.MaxValue)]
        public int CityId { get; set; }

        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }

        //obtenemos el id del departamento
        public virtual Department Department { get; set; }
        //obtenemos el id de la ciudad
        public virtual City City { get; set; }

        //Relacion con la tabla category 
        public virtual ICollection<Category> Categories { get; set; }

        //Relacion con la tabla tax 
        public virtual ICollection<Tax> Taxes { get; set; }

        //Relacion con la tabla Products 
        public virtual ICollection<Product> Products { get; set; }
        //relacion con la tabla Warehouse
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        //relacion con la tabla Customers
        public virtual ICollection<Customer> Customers { get; set; }
        //relacion con la tabla Orders
        public virtual ICollection<Order> Orders { get; set; }
    }
}