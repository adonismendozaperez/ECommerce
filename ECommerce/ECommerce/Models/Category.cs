using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Category")]
        [Index("CategoryName", IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        //obtenemos el id de company
        public virtual Company Company { get; set; }

        //Relacion con la tabla Products 
        public virtual ICollection<Product> Products { get; set; }

    }
}