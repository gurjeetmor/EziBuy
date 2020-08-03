using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    public class RegistrationAndLoginController : Controller
    {
        public UserDbContext userDbContext = new UserDbContext();
        // GET: RegistrationAndLogin
        public ActionResult Index()
        {          
            return View();
        }

        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        //GET:Registration
        public ActionResult Registration()
        {
            return View();
        }
        //POST:Registration
        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                var isEmailExist = userDbContext.UserContext.Where(x => x.EmailId == user.EmailId).FirstOrDefault();
                if(isEmailExist==null)
                {
                    user.RoleId = 2;
                    userDbContext.UserContext.Add(user);                    
                    userDbContext.SaveChanges();
                    ModelState.Clear();
                    MigrateShoppingCart(user.EmailId);
                    ViewBag.Message = user.FirstName + " " + user.LastName + " " + "Successfully Register";
                }
                else
                {
                    ViewBag.Message = "Email Id not available ! Please select another one.";
                }                            
            }
            return View();
        }
       
        [HttpPost]
        public JsonResult CheckEmail(string useremail)
        {
            //System.Threading.Thread.Sleep(5);
            UserDbContext userDbContext = new UserDbContext();
            bool result = userDbContext.UserContext.ToList().Exists(model => model.EmailId.Equals(useremail, StringComparison.CurrentCultureIgnoreCase));
            return Json(result);

        }
                
        //GET:Login
        public ActionResult Login()
        {
            LoginDetail loginDetail = new LoginDetail();
            if (Request.Cookies["LoginCookie"] != null)
            {
                loginDetail.EmailId = Request.Cookies["LoginCookie"].Values["EmailId"];
                //model.Password = Request.Cookies["Login"].Values["Password"];
            }
            return View(loginDetail);

            //return View();
        }
        //POST:Login
       
        [HttpPost]
        public ActionResult Login(LoginDetail loginDetail)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                bool isValid = userDbContext.UserContext.Any(x => x.EmailId == loginDetail.EmailId && x.Password == loginDetail.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(loginDetail.EmailId, false);
                    Session["EmailId"] = loginDetail.EmailId.ToString();
                    MigrateShoppingCart(loginDetail.EmailId);
                    if (loginDetail.RememberMe)
                    {                       
                        HttpCookie cookie = new HttpCookie("LoginCookie");
                        //cookie.Values["lastLoginDate"] = DateTime.Now.ToShortDateString();
                        cookie.Values.Add("EmailId", loginDetail.EmailId.ToString());
                        cookie.Expires = DateTime.Now.AddMinutes(30);
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "Home");
                    }                   
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }                    
                }
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }          
        }
        
        public ActionResult About()
        {
            return View();
        }
        //GET:Main Page
        [Authorize(Roles = "Admin, User")]
        public ActionResult MainPage()
        {
            if (Session["EmailId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        //GET:Logout
        [Authorize(Roles = "Admin, User")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");           
        }       
    }
}