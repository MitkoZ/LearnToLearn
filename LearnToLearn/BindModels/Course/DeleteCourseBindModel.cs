using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnToLearn.BindModels.Course
{
    public class DeleteCourseBindModel
    {
        [Required]
        public string TeacherId { get; set; }
    }
}