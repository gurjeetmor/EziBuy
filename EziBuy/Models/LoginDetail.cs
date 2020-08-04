using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class LoginDetail
    {
        [Key]
        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Email-Id")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid Email address")]
        public string EmailId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Please enter atleast 6 characters")]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }

        //List<User> UserDetail { get; set; }

    }
}