using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnToLearn.Controllers
{
    public class MarkIsVisibleBindModel
    {
        [Required]
        public bool IsVisible { get; set; }
        [Required]
        public string TeacherId { get; set; }
    }
}