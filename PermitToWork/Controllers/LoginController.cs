using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PermitToWork.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index(string returnUrl, string e)
        {
            ViewBag.returnUrl = returnUrl;

            if (e != null)
            {
                switch (e)
                {
                    case "401":
                        ViewBag.m = "Another user already become supervisor for that Permit.";
                        break;
                    case "402":
                        ViewBag.m = "Another user already become facility owner for that Permit.";
                        break;
                    case "404":
                        ViewBag.m = "Something wrong with your link. Maybe you have try something?";
                        break;
                };
            }
            return PartialView();
        }

        [HttpPost]
        public JsonResult ValidateLogin(string username, string password)
        {
            UserEntity userLogin = new UserEntity(username, EncodePassword(password));

            if (userLogin.isSuccessLogin == false)
            {
                return Json(new { status = "500", message = "Login Failed. Either your username or your password isn't exist in system. Please check again." });
            }

            Session["user"] = userLogin;
            FormsAuthentication.SetAuthCookie(userLogin.username, false);
            return Json(new { status = "200", message = "" });
        }

        private string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}
