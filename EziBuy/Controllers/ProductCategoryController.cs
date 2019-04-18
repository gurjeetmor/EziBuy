using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }
        //Product Category Information
        [HttpGet]
        public ActionResult DisplayProductCategory()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var productCategoryList = userDbContext.ProductCategoryContext.ToList();
                ViewBag.categoryList = productCategoryList;
                return View(productCategoryList);
            }
        }

        [HttpPost]
        public ActionResult DisplayProductCategory(ProductCategory productCategory)
        {
            try
            {
                using (UserDbContext userDbContext = new UserDbContext())
                {
                    if (productCategory.Id > 0)
                    {
                        ProductCategory categoryDetail = userDbContext.ProductCategoryContext.Where(x => x.Id == productCategory.Id).FirstOrDefault();
                        categoryDetail.CategoryName = productCategory.CategoryName;
                        userDbContext.SaveChanges();
                    }
                    else
                    {
                        var isProductCategoryExist = userDbContext.ProductCategoryContext.Where(x => x.CategoryName == productCategory.CategoryName).SingleOrDefault();
                        if (isProductCategoryExist == null)
                        {
                            userDbContext.ProductCategoryContext.Add(productCategory);
                            userDbContext.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Message = "Product Category is already exist";
                        }
                    }
                    return RedirectToAction("DisplayProductCategory");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EditProductCategory(int CategoryId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                ProductCategory productCategory = new ProductCategory();
                if (CategoryId > 0)
                {
                    var categoryInfo = userDbContext.ProductCategoryContext.Where(x => x.Id == CategoryId).FirstOrDefault();
                    productCategory.Id = categoryInfo.Id;
                    productCategory.CategoryName = categoryInfo.CategoryName;
                }
                return PartialView("CategoryPartial", productCategory);
            }

        }

        public ActionResult DeleteCategory(int categoryId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                ProductCategory productCategory = userDbContext.ProductCategoryContext.Where(x => x.Id == categoryId).SingleOrDefault();                //bool result = false;
                if (productCategory != null)
                {
                    userDbContext.ProductCategoryContext.Remove(productCategory);
                    userDbContext.SaveChanges();
                }
                return View("DisplayProductCategory");
            }

        }

        public ActionResult CategoryProductsList(int categoryId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                List<ProductDetail> productList = userDbContext.ProductDetailContext.Where(x => x.CategoryId == categoryId).ToList();
                //Query will use to access the selected product category name using passed categoryId
                //then pass the Category name to view to diplay in product list
                var categoryName = (from category in userDbContext.ProductCategoryContext
                                   where category.Id == categoryId
                                   select category.CategoryName).SingleOrDefault();               
                ViewBag.productCategoryName = categoryName;
                return View(productList);
            }

        }
    }
}