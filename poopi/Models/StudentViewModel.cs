using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace poopi.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string GroupName { get; set; }
        public string Email { get; set; }
    }
}