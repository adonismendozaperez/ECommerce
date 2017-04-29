using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage ="The field {0} is required.")]
        [MaxLength(50)]
        [Index("DepartmentName",IsUnique =true)]
        [Display(Name ="Department")]
        public string Name { get; set; }

        //relacion con la tabla ciudad
        public virtual ICollection<City> Cities { get; set; }
        //relacion con la tabla compania
        public virtual ICollection<Company> Companies { get; set; }
        //relacion con la tabla Usuario
        public virtual ICollection<User> Users { get; set; }
        //relacion con la tabla Warehouse
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}