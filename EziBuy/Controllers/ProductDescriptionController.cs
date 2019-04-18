using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductDescriptionController : Controller
    {
        // GET: ProductDescription
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DisplayProductDescriptionList()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productDescriptionList = userDbContext.ProductDescriptionContext.ToList();
                ViewBag.productList = productDescriptionList;
                return View(productDescriptionList);
            }
        }

        [HttpGet]
        public ActionResult CreateProductDescription()
        {
            UserDbContext userDbContext = new UserDbContext();
            //This viewBag use to pass the product category as a list to view where user can select 
            //product category from dropdown list
            ViewBag.ProductDetails = userDbContext.ProductDetailContext.ToList();
            return PartialView("CreateProductDescriptionPartial", new ProductViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateProductDescription(ProductViewModel productViewModel)
        {
            UserDbContext userDbContext = new UserDbContext();
            var validImageTypes = new string[]
                {
                 "image/gif",
                 "image/jpeg",
                 "image/bmp",
                 "image/png",
                 "image/jpg"
                };
            if (productViewModel.ImageUpload == null || productViewModel.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(productViewModel.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG, JPEG, BMP or PNG image.");
            }
            var productDetail = userDbContext.ProductDetailContext.Where(x => x.Id == productViewModel.ProductId).SingleOrDefault();
            if (!ModelState.IsValid)
            {
                ProductDescriptionModel productDescription = new ProductDescriptionModel
                {
                    ProductId = productViewModel.ProductId,
                    ProductName = productViewModel.ProductName,                    
                    Price = productViewModel.Price
                };

                if (productViewModel.ImageUpload != null && productViewModel.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/ProductImages/";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), productViewModel.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, productViewModel.ImageUpload.FileName);
                    productViewModel.ImageUpload.SaveAs(imagePath);
                    productDescription.ColorImageUrl = imageUrl;
                }
               
                userDbContext.ProductDescriptionContext.Add(productDescription);
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayProductDescriptionList");
            }
            return View("DisplayProductDescriptionList");
        }

        public ActionResult DeleteProduct(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                ProductDescriptionModel productDetail = userDbContext.ProductDescriptionContext.Where(x => x.Id == productId).SingleOrDefault();                //bool result = false;
                if (productDetail != null)
                {
                    userDbContext.ProductDescriptionContext.Remove(productDetail);
                    userDbContext.SaveChanges();
                }
                return View("DisplayProductDescriptionList");
            }

        }

        [HttpGet]
        public ActionResult EditProductDetail(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productInfo = userDbContext.ProductDescriptionContext.Find(productId);
                if (productInfo == null)
                {
                    return new HttpNotFoundResult();
                }
                ViewBag.ProductId = productId;
                //pass the product category list
                ViewBag.ProductList = userDbContext.ProductDetailContext.ToList();
                var productViewModel = new ProductViewModel
                {
                    ProductName = productInfo.ProductName,
                    ImageUrl = productInfo.ColorImageUrl,
                    Price = productInfo.Price,
                    ProductId = productInfo.ProductId,
                };
                return PartialView("EditProductDescriptionPartial", productViewModel);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditProductDetail(int productId, ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                UserDbContext userDbContext = new UserDbContext();
                var productDetail = userDbContext.ProductDescriptionContext.Find(productId);
                if (productDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                productDetail.Id = productId;
                productDetail.ProductName = productViewModel.ProductName;
                productDetail.Price = productViewModel.Price;
                productDetail.ProductId = productViewModel.ProductId;
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayProductDescriptionList");
            }
            return RedirectToAction("DisplayProductDescriptionList");
        }       

        [HttpGet]
        public ActionResult EditProductImage(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productInfo = userDbContext.ProductDescriptionContext.Find(productId);
                if (productInfo == null)
                {
                    return new HttpNotFoundResult();
                }
                ViewBag.productId = productId;
                var productViewModel = new ProductViewModel
                {
                    ImageUrl = productInfo.ColorImageUrl,
                };
                return PartialView("EditProductColorImagePartial", productViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductImage(int productId, ProductViewModel productViewModel)
        {
            var validImageTypes = new string[]
            {
                 "image/gif",
                 "image/jpeg",
                 "image/jpg",
                 "image/png",
                 "image/bmp"
            };
            if (productViewModel.ImageUpload != null || productViewModel.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(productViewModel.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }
            if (!ModelState.IsValid)
            {
                UserDbContext userDbContext = new UserDbContext();
                var productDetail = userDbContext.ProductDescriptionContext.Find(productId);
                if (productDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                productDetail.Id = productId;

                if (productViewModel.ImageUpload != null && productViewModel.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/ProductImages";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), productViewModel.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, productViewModel.ImageUpload.FileName);
                    productViewModel.ImageUpload.SaveAs(imagePath);
                    productDetail.ColorImageUrl = imageUrl;
                }
                else
                {
                    productDetail.ColorImageUrl = productViewModel.ImageUrl;

                }
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayProductDescriptionList");
            }
            return RedirectToAction("DisplayProductDescriptionList");
        }
    }
}