using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EziBuy.Models;

namespace EziBuy.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        [HttpGet]
        public ActionResult UserInformation()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {

                if (Session["EmailId"] != null)
                {
                    var emailId = Session["EmailId"].ToString();
                    var userInformation = userDbContext.UserContext.Where(x => x.EmailId == emailId).FirstOrDefault();
                    return View(userInformation);
                }
                else
                {
                    return RedirectToAction("Index", "RegistrationAndLogin");

                }
            }
        }

        
        public ActionResult EditUserInformation(User user)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var userInformation = userDbContext.UserContext.Where(x => x.Id == user.Id).FirstOrDefault();
                if (userInformation != null)
                {
                    userInformation.FirstName = user.FirstName;
                    userInformation.LastName = user.LastName;
                    userInformation.EmailId = user.EmailId;
                    userInformation.Gender = user.Gender;
                    userInformation.DateOfBirth = user.DateOfBirth;
                    userInformation.Password = user.Password;
                    //userDbContext.UserContext.Add(user);
                    userDbContext.SaveChanges();

                }
                return RedirectToAction("UserInformation");
            }
        }



    }
}