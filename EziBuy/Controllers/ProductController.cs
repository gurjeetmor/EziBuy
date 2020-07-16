using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DisplayProductList()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productList = userDbContext.ProductDetailContext.ToList();
                ViewBag.productDetailList = productList;
                return View(productList);
            }
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            UserDbContext userDbContext = new UserDbContext();
            
            ViewBag.ItemsBag = userDbContext.SizeContext.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString() // implies all values are strings
            }).ToList();
            //This viewBag use to pass the product category as a list to view where user can select 
            //product category from dropdown list
            ViewBag.ListofCategory = userDbContext.ProductCategoryContext.ToList();
            return PartialView("CreateProductPartial", new ProductViewModel());
        }

        public List<Size> GetAllSizeTypes()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var sizeDetail = userDbContext.SizeContext.ToList();
                return sizeDetail;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductViewModel productViewModel)
        {
            productViewModel.Sizes = GetAllSizeTypes();
            string sizeNames = string.Empty;
            if (productViewModel.SizeIds != null)
            {
                var selectedItems = productViewModel.Sizes.Where(p => productViewModel.SizeIds.Contains((int)(p.Id))).ToList();
                foreach (var selectedItem in selectedItems)
                {
                    sizeNames += selectedItem.Id + " ";
                }
            }
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
            if (!ModelState.IsValid)
            {
                ProductDetail productDetail = new ProductDetail
                {
                    ProductName = productViewModel.ProductName,
                    AltText = productViewModel.AltText,
                    Caption = productViewModel.Caption,
                    ProductDescription = productViewModel.ProductDescription,
                    Price = productViewModel.Price,
                    Size = sizeNames,
                    Quantity = productViewModel.Quantity,
                    CategoryId = productViewModel.CategoryId
                };

                if (productViewModel.ImageUpload != null && productViewModel.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/ProductImages/";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), productViewModel.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, productViewModel.ImageUpload.FileName);
                    productViewModel.ImageUpload.SaveAs(imagePath);
                    productDetail.ImageUrl = imageUrl;
                }
                UserDbContext userDbContext = new UserDbContext();
                userDbContext.ProductDetailContext.Add(productDetail);
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayProductList");
            }
            return View("DisplayProductList");
        }

        public ActionResult DeleteProduct(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                ProductDetail productDetail = userDbContext.ProductDetailContext.Where(x => x.Id == productId).SingleOrDefault();                //bool result = false;
                if (productDetail != null)
                {
                    userDbContext.ProductDetailContext.Remove(productDetail);
                    userDbContext.SaveChanges();
                }
                return View("DisplayProductList");
            }

        }

        [HttpGet]
        public ActionResult EditProductDetail(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productInfo = userDbContext.ProductDetailContext.Find(productId);
                if (productInfo == null)
                {
                    return new HttpNotFoundResult();
                }
                ViewBag.productId = productId;
                //pass the product category list
                ViewBag.ListofCategory = userDbContext.ProductCategoryContext.ToList();
                ViewBag.ItemsBag = userDbContext.SizeContext.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString() // implies all values are strings
                }).ToList();

                var selectedSizes = productInfo.Size.Select(s => Int32.TryParse(s.ToString(), out int n) ? n : (int?)null).ToList();


                var selectedItems = userDbContext.SizeContext.Where(p => selectedSizes.Contains((int)(p.Id))).ToList();               
                var productViewModel = new ProductViewModel
                {
                    ProductName = productInfo.ProductName,
                    ImageUrl = productInfo.ImageUrl,
                    AltText = productInfo.AltText,
                    Caption = productInfo.Caption,
                    ProductDescription = productInfo.ProductDescription,
                    Price = productInfo.Price,
                    CategoryId = productInfo.CategoryId,
                    SizesSelected = selectedItems.Select(v => new SelectListItem
                    {
                        Text = v.Name,
                        Value = v.Id.ToString(),
                        Selected = true
                    }).ToList(),
                    Quantity = productInfo.Quantity
            };
                return PartialView("EditProductInformationPartial", productViewModel);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditProductDetail(int productId, ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                UserDbContext userDbContext = new UserDbContext();
                var productDetail = userDbContext.ProductDetailContext.Find(productId);

                if (productDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                productViewModel.Sizes = GetAllSizeTypes();
                string sizeNames = string.Empty;
                if (productViewModel.SizeIds != null)
                {
                    var selectedItems = productViewModel.Sizes.Where(p => productViewModel.SizeIds.Contains((int)(p.Id))).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        sizeNames += selectedItem.Id + " ";
                    }
                }
                productDetail.Id = productId;
                productDetail.ProductName = productViewModel.ProductName;
                productDetail.AltText = productViewModel.AltText;
                productDetail.Caption = productViewModel.Caption;
                productDetail.Price = productViewModel.Price;
                productDetail.ProductDescription = productViewModel.ProductDescription;
                productDetail.Size = sizeNames;
                productDetail.Quantity = productViewModel.Quantity;
                productDetail.CategoryId = productViewModel.CategoryId;
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayProductList");
            }
            return RedirectToAction("DisplayProductList");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditProductDetail(int productId, ProductViewModel productViewModel)
        //{
        //    var validImageTypes = new string[]
        //    {
        //         "image/gif",
        //         "image/jpeg",
        //         "image/jpg",
        //         "image/png",
        //         "image/bmp"
        //    };
        //    for (int i = 0; i < Request.Files.Count; i++)
        //    {
        //        var file = Request.Files[i];
        //    }

        //    if (productViewModel.ImageUpload != null || productViewModel.ImageUpload.ContentLength > 0 && !validImageTypes.Contains(productViewModel.ImageUpload.ContentType))
        //    {
        //        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
        //    }


        //    if (!ModelState.IsValid)
        //    {
        //        UserDbContext userDbContext = new UserDbContext();
        //        var productDetail = userDbContext.ProductDetailContext.Find(productId);
        //        if (productDetail == null)
        //        {
        //            return new HttpNotFoundResult();
        //        }
        //        productDetail.Id = productId;
        //        productDetail.ProductName = productViewModel.ProductName;
        //        productDetail.AltText = productViewModel.AltText;
        //        productDetail.Caption = productViewModel.Caption;
        //        productDetail.ProductDescription = productViewModel.ProductDescription;
        //        productDetail.CategoryId = productViewModel.CategoryId;

        //        if (productViewModel.ImageUpload != null && productViewModel.ImageUpload.ContentLength > 0)
        //        {
        //            for (int i = 0; i < Request.Files.Count; i++)
        //            {
        //                var file = Request.Files[i];
        //                var fileName = Path.GetFileName(file.FileName);
        //                var uploadDir = "~/ProductImages";
        //                var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
        //                var imageUrl = Path.Combine(uploadDir, fileName);
        //                productViewModel.ImageUpload.SaveAs(imagePath);
        //                productDetail.ImageUrl = imageUrl;
        //            }

        //        }
        //        else
        //        {
        //            productDetail.ImageUrl = productViewModel.ImageUrl;

        //        }
        //        userDbContext.SaveChanges();
        //        return RedirectToAction("DisplayProductList");
        //    }
        //    return RedirectToAction("DisplayProductList");
        //}

        [HttpGet]
        public ActionResult EditProductImage(int productId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productInfo = userDbContext.ProductDetailContext.Find(productId);
                if (productInfo == null)
                {
                    return new HttpNotFoundResult();
                }
                ViewBag.productId = productId;
                var productViewModel = new ProductViewModel
                {
                    ImageUrl = productInfo.ImageUrl,
                };
                return PartialView("EditProductImagePartial", productViewModel);
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
                var productDetail = userDbContext.ProductDetailContext.Find(productId);
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
                    productDetail.ImageUrl = imageUrl;
                }
                else
                {
                    productDetail.ImageUrl = productViewModel.ImageUrl;

                }
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayProductList");
            }
            return RedirectToAction("DisplayProductList");
        }       
    }
}


