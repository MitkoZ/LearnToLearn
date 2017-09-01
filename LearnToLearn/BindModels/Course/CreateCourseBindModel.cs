using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnToLearn.ViewModels
{
    public class CreateCourseBindModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string TeacherId { get; set; }
    }
}