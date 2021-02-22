using Microsoft.AspNet.Identity;
using poopi.Entities.Models;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poopi.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "AdminPanel", new { area = "admin" });
                if (User.IsInRole("Student"))
                    return RedirectToAction("Index", "StudentPanel", new { area = "student" });
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "User")]
        public ActionResult GetAllGroups()
        {
            IEnumerable<GroupViewModel> groups = _context.Groups.Select(x => new GroupViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Course = x.Course
            });
            return View(groups);
        }
        [Authorize(Roles = "User")]
        public ActionResult Request(string id)
        {
            string userid = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.Find(userid);
            Group group = _context.Groups.FirstOrDefault(el => el.Id.ToString() == id);
            Student student = user.Student;

            Request request = new Request();
            request.GroupId = group.Id;
            request.StudentId = student.Id;

            _context.Requests.Add(request);
            _context.SaveChanges();
            return RedirectToAction("GetAllGroups");
        }
    }
}