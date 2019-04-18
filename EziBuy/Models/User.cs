using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EziBuy.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "First Name contain letters only")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Last Name contain letters only")]
        public string LastName { get; set; }

        [Display(Name = "Email-Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Email-Id")]
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        [EmailAddress(ErrorMessage = "Please enter valid Email address")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid Email address")]
        public string EmailId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select one option")]
        public string Gender { get; set; }

        [Display(Name = " Date of Birth")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Date of Birth")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode =true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Please enter atleast 6 characters")]
        public string Password { get; set; }

        public int RoleId { get; set; }

        //public virtual UserRole userRole { get; set; }

        //List<UserRole> userRoles { get; set; }
    }
}