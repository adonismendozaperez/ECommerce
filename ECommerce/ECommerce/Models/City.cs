using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name ="City")]
        [Index("CityName",2,IsUnique =true)]
        public string Name { get; set; }

        [Required]
        [Range(1,double.MaxValue)]
        [Index("CityName",1, IsUnique = true)]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        
        //obtenemos el id de departamento
        public virtual Department Department { get; set; }
        
        //Relacion con la tabla Compania 
        public virtual ICollection<Company> Companies { get; set; }

        //Relacion con la tabla Compania 
        public virtual ICollection<User> Users { get; set; }
        //relacion con la tabla Warehouse
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}