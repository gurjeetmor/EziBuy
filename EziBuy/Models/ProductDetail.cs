using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product name is required field")]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select image")]
        [Display(Name ="Image URL")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter value")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Description is required field")]
        [Display(Name ="Product Description")]        
        [MaxLength(1000, ErrorMessage ="Description should be less than 1000 characters")]
        public string ProductDescription { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Please select the product category")]
        public int CategoryId { get; set; }

        public string Size { get; set; }

        public int? Quantity { get; set; }

        List<ProductImage> ProductImages { get; set; }

        List<ProductDescriptionModel> ProductsDescriptionModel { get; set; }
    }
}