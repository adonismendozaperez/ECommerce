using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(256)]
        [Display(Name = "E-Mail")]
        [Index("UserName", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(20)]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(200)]
        public string Address { get; set; }


        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Range(1, double.MaxValue)]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }


        [Required(ErrorMessage = "The field {0} is required.")]
        [Range(1, double.MaxValue)]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [Range(1, double.MaxValue)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "User")]
        public string FullName { get {return string.Format("{0} {1}",FirstName,LastName); } }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        //obtenemos el id del departamento
        public virtual Department Department { get; set; }
        //obtenemos el id de la ciudad
        public virtual City City { get; set; }
        //obtenemos el id de la compania
        public virtual Company Company { get; set; }
    }
}