using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class ProductDescriptionModel
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This is required field")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select image")]
        [Display(Name = "Color Image URL")]
        [DataType(DataType.ImageUrl)]
        public string ColorImageUrl { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter value")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select the product category")]
        public int ProductId { get; set; }
    }
}