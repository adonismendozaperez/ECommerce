using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name ="Department")]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}