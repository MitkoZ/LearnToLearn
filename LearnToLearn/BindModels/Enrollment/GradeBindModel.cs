using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnToLearn.BindModels.Enrollment
{
    public class GradeBindModel
    {
        [Required]
        [Range(2, 6, ErrorMessage = "Grade must be between 2 and 6")]
        public double Grade { get; set; }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string TeacherId { get; set; }
    }
}