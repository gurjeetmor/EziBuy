using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //this method pass the products category list to shared partial view to display the 
        //category list in left menu bar directly from the categories stored in database

        public ActionResult CategoryNavBarList()
        {
            using(UserDbContext userDbContext= new UserDbContext())
            {
                var categoryBarList= userDbContext.ProductCategoryContext.ToList();
                return PartialView("~/Views/Shared/CategoryNavBar.cshtml", categoryBarList);
            }
        }

        public ActionResult CategoriesList()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var categoryList = userDbContext.ProductCategoryContext.ToList();
                return PartialView("~/Views/Shared/CategoriesList.cshtml", categoryList);
            }
        }
      
        //method display the products related to particular category when click on particular category
        public ActionResult BrowseCategoryProduct(int categoryId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var categoryProducts = userDbContext.ProductDetailContext.Where(x => x.CategoryId == categoryId).ToList();
                return View(categoryProducts);
            }
        }
        
        //this method is used to diplay the Carousel. method will pass the Carousel Detail list
        //to partial view define in shared folder
        public ActionResult Carousel()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var carouselList = userDbContext.HomeCarouselContext.ToList();
                return PartialView("~/Views/Shared/CarouselView.cshtml", carouselList);
            }
        }

        public ActionResult ProductDescription(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                TempData["ProductId"] = productId;
                var productImages = userDbContext.ProductImageContext.Where(x => x.ProductId == productId).ToList();
                
                    //this query return the First image Url from productImage table that match
                    //with the productId and this URL used as by Default image URL when user browse
                    //the product description after that image can be change as user mouseover the other product images display on left side
                    var productDefaultImage = (from productImage in userDbContext.ProductImageContext
                                               where productImage.ProductId == productId
                                               select productImage.ImageUrl).FirstOrDefault();
                    ViewBag.defaultImage = productDefaultImage;               
                return View(productImages);                                        
            }            
        }  
        public ActionResult Details()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productId = Convert.ToInt32(TempData["ProductId"]);
                ViewBag.productDetail = userDbContext.ProductDetailContext.Where(x => x.Id == productId).SingleOrDefault();
                return PartialView("DetailsPartial", new ProductViewModel());
            }
        }
    }
}