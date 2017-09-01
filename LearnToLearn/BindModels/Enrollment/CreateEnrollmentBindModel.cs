using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnToLearn.BindModels
{
    public class CreateEnrollmentBindModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}