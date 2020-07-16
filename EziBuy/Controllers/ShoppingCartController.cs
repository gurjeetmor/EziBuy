using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    public class ShoppingCartController: Controller
    {
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddToCart(ProductViewModel productViewModel)
        {
            return View();
        }
    }
}