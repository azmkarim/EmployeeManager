using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.ApiClient.Models
{
    public class Employee
    {
        [Required(ErrorMessage = "Employee ID Required!")]
        [Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name Is Required")]
        [Display(Name = "First Name")]
        [StringLength(10, ErrorMessage = "First Name Must Be Less Then 10 Character")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(20, ErrorMessage = "Last Name Must Be Less Then 20 Character")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Title Is Required!")]
        [Display(Name = "Title")]
        [StringLength(30, ErrorMessage = "Title Must Be Less Then 30 Character")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country Name Is Required !")]
        [StringLength(15, ErrorMessage = "Country Must Be Less Then 15 Character")]
        public string Country { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes Must Be Less Then 500 Character")]
        public string Notes { get; set; }
    }
}
