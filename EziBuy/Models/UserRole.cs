using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        //[Required]
        //public int UserId { get; set; }

        [Required]
        public string RoleName { get; set; }

        List<User> user { get; set; }

        //public virtual User user { get; set; }




    }
}