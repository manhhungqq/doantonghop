using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qldoansvhutech.Areas.Admin.Models;
using qldoansvhutech.Models;

namespace qldoansvhutech.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
         QLDADataContext db = new QLDADataContext();
        // GET: Admin/Account
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost, ActionName("SignIn")]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AdminViewModel account, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                var isValid = db.Admins.SingleOrDefault(p => p.Taikhoan == account.Taikhoan);
                if (isValid == null)
                {
                    ViewBag.Message = "Tài khoản không tồn tại.";
                    return View(account);
                }
                if(isValid.Role == null)
                {
                    ViewBag.Message = "Tài khoản bị khóa.";
                    return View(account);
                }
                else if (isValid.Matkhau != account.Matkhau)
                {
                    ViewBag.Message = "Mật khẩu bị sai.";
                    return View(account);
                }
                HttpCookie userCookie = new HttpCookie("AdminAccount", account.Taikhoan);
                userCookie["ID"] = isValid.Id.ToString();
                userCookie["Username"] = account.Taikhoan;
                userCookie["Password"] = account.Matkhau;
                userCookie.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(userCookie);
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(account);
        }
        public ActionResult SignOut()
        {
            HttpCookie userCookie = new HttpCookie("AdminAccount");
            userCookie.Expires = DateTime.Now.AddDays(-1);
            Response.SetCookie(userCookie);
            return RedirectToAction("SignIn", "Account");
        }
    }
}
