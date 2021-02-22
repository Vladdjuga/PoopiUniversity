using Microsoft.AspNet.Identity;
using poopi.Entities.Models;
using poopi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            GroupViewModel res = new GroupViewModel() {
                Course = _context.Users.Find(User.Identity.GetUserId()).Student.Group.Course,
                Name = _context.Users.Find(User.Identity.GetUserId()).Student.Group.Name,
                Students = _context.Users.Find(User.Identity.GetUserId()).Student.Group!=null ? _context.Users.Find(User.Identity.GetUserId()).Student.Group.Students : null
            };

            return View(res);
        }
    }
}