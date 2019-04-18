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
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //method to display the list of registered users
        [HttpGet]
        public ActionResult UsersDetail()
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                var userList = userDbContext.UserContext.ToList();
                ViewBag.userDetailList = userList;
                return View(userList);
            }
        }

        //This method allow the Admin to add new user or update the details of already registered user
        [HttpPost]
        public ActionResult UsersDetail(User user)
        {
            try
            {
                using (UserDbContext userDbContext = new UserDbContext())
                {
                    //this condition checks either user already register or not using Id value
                    //if Id is greater than 0 that means user already register and their details can be updated
                    //else admin can add the new user
                    if (user.Id > 0)
                    {
                        User userInformation = userDbContext.UserContext.Where(x => x.Id == user.Id).FirstOrDefault();
                        userInformation.FirstName = user.FirstName;
                        userInformation.LastName = user.LastName;
                        userInformation.EmailId = user.EmailId;
                        userInformation.Gender = user.Gender;
                        userInformation.DateOfBirth = user.DateOfBirth;
                        userInformation.Password = user.Password;
                        userDbContext.SaveChanges();
                    }
                    else
                    {                 
                        var isEmailExist = userDbContext.UserContext.Where(x => x.EmailId == user.EmailId).SingleOrDefault();
                        if (isEmailExist == null)
                        {
                            user.RoleId = 2;
                            userDbContext.UserContext.Add(user);
                            userDbContext.SaveChanges();
                        }
                        else
                        {                            
                            ViewBag.Message = "Email Id not available ! Please select another one.";
                        }
                    }
                    return RedirectToAction("UsersDetail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult CheckEmail(string useremail)
        {           
            UserDbContext userDbContext = new UserDbContext();
            bool result = userDbContext.UserContext.ToList().Exists(model => model.EmailId.Equals(useremail, StringComparison.CurrentCultureIgnoreCase));
            return Json(result);
        }

        public ActionResult EditUserInformation(int UserId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                User user = new User();
                if (UserId > 0)
                {
                    var userInfo = userDbContext.UserContext.Where(x => x.Id == UserId).FirstOrDefault();
                    user.Id = userInfo.Id;
                    user.FirstName = userInfo.FirstName;
                    user.LastName = userInfo.LastName;
                    user.EmailId = userInfo.EmailId;
                    user.Gender = userInfo.Gender;
                    user.DateOfBirth = userInfo.DateOfBirth;
                    user.Password = userInfo.Password;
                }
                return PartialView("EditUserInformationPartial", user);
            }
        }

        public ActionResult DeleteUser(int userId)
        {
            using (UserDbContext userDbContext = new UserDbContext())
            {
                User user = userDbContext.UserContext.Where(x => x.Id == userId).SingleOrDefault();                //bool result = false;
                if (user != null)
                {
                    userDbContext.UserContext.Remove(user);
                    userDbContext.SaveChanges();
                }
                return View("UsersDetail");
            }
        }      
    }
}