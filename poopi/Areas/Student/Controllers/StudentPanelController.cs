using Microsoft.AspNet.Identity;
using poopi.Entities.Models;
using poopi.Helpers;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace poopi.Areas.Student.Controllers
{
    public class StudentPanelController : Controller
    {
        private ApplicationDbContext _context;
        public StudentPanelController()
        {
            _context = new ApplicationDbContext();
        }
        [Authorize(Roles = "Student")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult GetMyGroup()
        {
            var group = _context.Users.Find(User.Identity.GetUserId()).Student.Group;
            var students = group != null ? group.Students : null;
            GroupViewModel res = new GroupViewModel()
            {
                Course = _context.Users.Find(User.Identity.GetUserId()).Student.Group.Course,
                Name = _context.Users.Find(User.Identity.GetUserId()).Student.Group.Name,
                Students = students
            };

            return View(res);
        }
        [HttpPost]
        public ActionResult ChangePic(HttpPostedFileBase httpfile)
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
            _context.SaveChanges();

            return RedirectToAction("Index", "StudentManage");
        }
    }
}