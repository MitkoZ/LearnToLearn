using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User : IdentityUser, IBaseEntity<string>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public override string Email { get; set; }

        public bool IsTeacher { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }

        public virtual List<Course> Courses { get; set; }

        public User()
        {
            Enrollments = new List<Enrollment>();
            Courses = new List<Course>();
        }
    }
}
