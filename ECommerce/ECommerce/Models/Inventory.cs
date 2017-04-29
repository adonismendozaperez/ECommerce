using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Inventory
    {

        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int WarehouseId { get; set; }


        [Required]
        public int ProductId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        //obtenemos el id del warehouses
        public virtual Warehouse warehouses { get; set; }
        //obtenemos el id del products
        public virtual Product products { get; set; }

        
    }
}