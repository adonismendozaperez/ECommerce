using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Tax")]
        [Index("TaxName", IsUnique = true)]
        public string Name { get; set; }

        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString ="{0:P2}",ApplyFormatInEditMode =false)]
        [Range(0,1,ErrorMessage ="You must select a {0} between {1} and {2}")]
        public double Rate { get; set; }

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