using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace poopi.Entities.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int GroupId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Group Group { get; set; }
    }
}