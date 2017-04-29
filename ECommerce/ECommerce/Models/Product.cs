using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }


        [Required]
        [MaxLength(50)]
        [Display(Name = "Product")]
        [Index("ProductName", IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [MaxLength(13)]
        [Display(Name = "Bar Code")]
        [Index("ProductBarCode", IsUnique = true)]
        public string BarCode { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Display(Name = "Tax")]
        public int TaxId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get {return inventories.Sum(c=>c.Stock); } }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFiler { get; set; }

        
        //obtenemos el id de company
        public virtual Company Company { get; set; }

        //obtenemos el id de category
        public virtual Category Category { get; set; }

        //obtenemos el id de Tax
        public virtual Tax Tax { get; set; }

        public virtual ICollection<Inventory> inventories { get; set; }
    }
}