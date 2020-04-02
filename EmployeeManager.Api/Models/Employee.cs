using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManager.Api.Models
{
    public partial class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EmployeeID")]
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
        [Column(TypeName = "datetime")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        [Column(TypeName = "datetime")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Country Name Is Required !")]
        [StringLength(15, ErrorMessage = "Country Must Be Less Then 15 Character")]
        public string Country { get; set; }

        [StringLength(500, ErrorMessage = "Notes Must Be Less Then 500 Character")]
        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
    }
}
