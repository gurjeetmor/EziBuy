using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EziBuy.Models
{
    public class CartItem
    {
        public ProductDetail Product { get; set; }
        public int ProductQuantity { get; set; }
    }
}