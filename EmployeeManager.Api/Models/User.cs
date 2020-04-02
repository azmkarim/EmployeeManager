using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Api.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(20, ErrorMessage = "User Name Must Be Less Then 20 Characters")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(20, ErrorMessage = "Password Must Be Less Then 20 Character")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "E-Mail")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full Name Must Be Less Then 100 Character")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Role")]
        public string Role { get; set; }

    }
}
