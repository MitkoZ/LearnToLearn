using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnToLearn.BindModels.Enrollment
{
    public class ShowEnrollmentBindModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public double Grade { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}