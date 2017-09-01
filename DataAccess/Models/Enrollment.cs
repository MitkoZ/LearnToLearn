using LearnToLearn.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Enrollment : BaseEntity, IBaseEntity<int>
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        [Required]
        [CustomRange(ErrorMessage = "Grade must be either between 2 and 6 or -1 (default value)", Inclusion = -1, MinValue = 2, MaxValue = 6)]
        public double Grade { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        public virtual Course Course { get; set; }
        public Enrollment()
        {
            Grade = -1;
        }
    }
}
