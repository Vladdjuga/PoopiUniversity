using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using poopi.Entities.Models;

namespace poopi.Models
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Course { get; set; }
        public IEnumerable<Student> Students { get; internal set; }
    }
}