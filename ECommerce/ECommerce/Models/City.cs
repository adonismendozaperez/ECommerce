using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Name { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}