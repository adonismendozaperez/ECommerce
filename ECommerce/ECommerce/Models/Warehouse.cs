using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Range(1, double.MaxValue)]
        [Display(Name = "Company")]
        [Index("Warehouse_Company_Name", IsUnique = true)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        [Display(Name = "Warehouse")]
        [Index("WarehouseName", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(20)]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Range(1, double.MaxValue)]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Range(1, double.MaxValue)]
        [Display(Name = "City")]
        public int CityId { get; set; }

        //obtenemos el id del departamento
        public virtual Department Department { get; set; }
        //obtenemos el id de la ciudad
        public virtual City City { get; set; }
        //obtenemos el id de la compania
        public virtual Company Company { get; set; }
        //relacion con la tabla inventory
        public virtual ICollection<Inventory> inventories { get; set; }


    }
}