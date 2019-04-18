using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarouselController : Controller
    {
        // GET: HomeCarouselImages
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DisplayCarouselList()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var carouselList = userDbContext.HomeCarouselContext.ToList();
                ViewBag.carouselDetailList = carouselList;
                return View(carouselList);
            }
        }

        [HttpGet]
        public ActionResult CreateCarousel()
        {                    
            return PartialView("CreateCarouselPartial", new ProductViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateCarousel(ProductViewModel productViewModel)
        {
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
                HomeCarousel homeCarousel = new HomeCarousel
                {                    
                    Caption = productViewModel.Caption,
                     AltText = productViewModel.AltText
                };

                if (productViewModel.ImageUpload != null && productViewModel.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/CarouselImages/";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), productViewModel.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, productViewModel.ImageUpload.FileName);
                    productViewModel.ImageUpload.SaveAs(imagePath);
                    homeCarousel.ImageUrl = imageUrl;
                }
                UserDbContext userDbContext = new UserDbContext();
                userDbContext.HomeCarouselContext.Add(homeCarousel);
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayCarouselList");
            }
            return View("DisplayCarouselList");
        }

        public ActionResult DeleteCarousel(int carouselId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                HomeCarousel homeCarouselDetail = userDbContext.HomeCarouselContext.Where(x => x.Id == carouselId).SingleOrDefault();                //bool result = false;
                if (homeCarouselDetail != null)
                {
                    userDbContext.HomeCarouselContext.Remove(homeCarouselDetail);
                    userDbContext.SaveChanges();
                }
                return View("DisplayCarouselList");
            }

        }

        [HttpGet]
        public ActionResult EditCarouselDetail(int carouselId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var carouselDetail = userDbContext.HomeCarouselContext.Find(carouselId);
                if (carouselDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                ViewBag.carouselId = carouselId;
                
                var productViewModel = new ProductViewModel
                {                  
                    ImageUrl = carouselDetail.ImageUrl,
                    AltText = carouselDetail.AltText,
                    Caption = carouselDetail.Caption,                    
                };
                return PartialView("EditCarouselDetailPartial", productViewModel);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditCarouselDetail(int carouselId, ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                UserDbContext userDbContext = new UserDbContext();
                var carouselDetail = userDbContext.HomeCarouselContext.Find(carouselId);
                if (carouselDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                carouselDetail.Id = carouselId;       
                carouselDetail.AltText = productViewModel.AltText;
                carouselDetail.Caption = productViewModel.Caption;
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayCarouselList");
            }
            return RedirectToAction("DisplayCarouselList");
        }

        [HttpGet]
        public ActionResult EditCarouselImage(int carouselId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var carouselDetail = userDbContext.HomeCarouselContext.Find(carouselId);
                if (carouselDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                ViewBag.carouselId = carouselId;
                var productViewModel = new ProductViewModel
                {
                    ImageUrl = carouselDetail.ImageUrl,
                };
                return PartialView("EditCarouselImagePartial", productViewModel);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditCarouselImage(int carouselId, ProductViewModel productViewModel)
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
                var carouselDetail = userDbContext.HomeCarouselContext.Find(carouselId);
                if (carouselDetail == null)
                {
                    return new HttpNotFoundResult();
                }
                carouselDetail.Id = carouselId;

                if (productViewModel.ImageUpload != null && productViewModel.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/CarouselImages";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), productViewModel.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, productViewModel.ImageUpload.FileName);
                    productViewModel.ImageUpload.SaveAs(imagePath);
                    carouselDetail.ImageUrl = imageUrl;
                }
                else
                {
                    carouselDetail.ImageUrl = productViewModel.ImageUrl;

                }
                userDbContext.SaveChanges();
                return RedirectToAction("DisplayCarouselList");
            }
            return RedirectToAction("DisplayCarouselList");
        }
    }
}