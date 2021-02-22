using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using poopi.Entities.Models;
using poopi.Helpers;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poopi.Areas.Admin.Controllers
{
    public class AdminPanelController : Controller
    {
        private ApplicationDbContext _context;

        public AdminPanelController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllStudents()
        {
            IEnumerable<StudentViewModel> student = _context.Students.Select(x => new StudentViewModel
            {
                Id = x.Id,
                Email = x.ApplicationUser.Email,
                GroupName = x.Group.Name,
                Image = x.Image,
                FullName=x.FullName
            });
            return View(student);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllGroups()
        {
            IEnumerable<GroupViewModel> groups = _context.Groups.Select(x => new GroupViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Course=x.Course
            });
            return View(groups);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllRequests()
        {
            IEnumerable<RequestViewModel> requests = _context.Requests.Select(x => new RequestViewModel
            {
                Id = x.Id,
                StudentEmail = x.Student.ApplicationUser.Email,
                GroupName = x.Group.Name
            });
            return View(requests);
        }
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
        public ActionResult Deny(int id)
        {
            Request request = _context.Requests.Find(id);
            request.Student.GroupId = request.Group.Id;
            UserManager.AddToRole(request.Student.ApplicationUser.Id,"Student");
            _context.Requests.Remove(request);
            _context.SaveChanges();
            return RedirectToAction("GetAllRequests");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreateGroupViewModel model)
        {
            Group group = new Group()
            {
                Name = model.Name,
                Course = model.Course
            };
            _context.Groups.Add(group);
            _context.SaveChanges();
            return RedirectToAction("GetAllGroups");
        }
        public ActionResult Edit(int id)
        {
            GroupViewModel model = new GroupViewModel();
            model.Name = _context.Groups.Find(id).Name;
            model.Course = _context.Groups.Find(id).Course;
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(GroupViewModel model)
        {
            Group group = _context.Groups.Find(model.Id);
            group.Name = model.Name;
            group.Course = model.Course;

            _context.SaveChanges();
            return RedirectToAction("GetAllGroups");
        }
        public ActionResult Delete(int id)
        {
            Group group = _context.Groups.Find(id);
            _context.Groups.Remove(group);
            _context.SaveChanges();
            return RedirectToAction("GetAllGroups");
        }
        public ActionResult EditStudent(int id)
        {
            StudentViewModel model = new StudentViewModel();
            model.Email = _context.Students.Find(id).ApplicationUser.Email;
            model.Image = _context.Students.Find(id).Image;
            //model.GroupName = _context.Students.Find(id).Group!=null? _context.Students.Find(id).Group.Name : "";
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStudent(StudentViewModel model,HttpPostedFileBase imagehttp)
        {
            string imagefile = Guid.NewGuid().ToString() + ".jpg";
            string image = Server.MapPath(ConstantsSec.ImagePath) + "\\" + imagefile;
            if (imagehttp != null)
            {
                using (Bitmap bitmap = new Bitmap(imagehttp.InputStream))
                {
                    var saved = ImageWorker.CreateImage(bitmap, 400, 400);
                    if (saved != null)
                    {
                        saved.Save(image, ImageFormat.Jpeg);
                    }
                }
            }
            else imagefile = "default.jpg";
            Entities.Models.Student student = _context.Students.Find(model.Id);
            student.ApplicationUser.Email = model.Email;
            student.Image = imagefile;
            //if (_context.Groups.FirstOrDefault(x => x.Name == model.GroupName) != null)
            //    student.GroupId = _context.Groups.FirstOrDefault(x => x.Name == model.GroupName).Id;

            _context.SaveChanges();
            return RedirectToAction("GetAllStudents");
        }
        public ActionResult DeleteStudent(int id)
        {
            Entities.Models.Student student = _context.Students.Find(id);
            ApplicationUser temp = _context.Users.FirstOrDefault(el => el.Student.Id == student.Id);
            _context.Users.Remove(temp);
            _context.Students.Remove(student);

            _context.SaveChanges();
            return RedirectToAction("GetAllStudents");
        }
    }
}