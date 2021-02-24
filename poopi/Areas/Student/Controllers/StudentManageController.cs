using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using poopi.Helpers;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static poopi.Controllers.ManageController;

namespace poopi.Areas.Student.Controllers
{
    public class StudentManageController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public StudentManageController()
        {
            _context = new ApplicationDbContext();
        }
        ApplicationDbContext _context;
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var student = _context.Users.Find(userId).Student;
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                Image = student != null ? student.Image : "default.jpg",
                FullName=student.FullName,
                Email=student.ApplicationUser.Email
            };
            return View(model);
        }
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [Authorize(Roles = "Student")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            return View(model);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult Edit(IndexEditViewModel model, HttpPostedFileBase httpfile)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());
            string imagefile = Guid.NewGuid().ToString() + ".jpg";
            string image = Server.MapPath(ConstantsSec.ImagePath) + "\\" + imagefile;
            if (httpfile != null)
            {
                using (Bitmap bitmap = new Bitmap(httpfile.InputStream))
                {
                    var saved = ImageWorker.CreateImage(bitmap, 400, 400);
                    if (saved != null)
                    {
                        saved.Save(image, ImageFormat.Jpeg);
                    }
                }
            }
            else imagefile = "default.jpg";
            user.Student.Image = imagefile;
            user.Email = model.Email;
            _context.Students.FirstOrDefault(el => el.Id == user.Student.Id).FullName = model.FullName;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}