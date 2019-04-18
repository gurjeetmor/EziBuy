using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Please enter product category name")]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }

        List<ProductDetail> ProductsDetail { get; set; }
    }
}