using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearnToLearn.Helpers
{
    public class CustomRangeAttribute : ValidationAttribute
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int Inclusion { get; set; }
        public override bool IsValid(object value)
        {
            double grade = (double)value;
            if (grade == Inclusion)
            {
                return true;
            }
            if (grade >= MinValue && grade <= MaxValue)
            {
                return true;
            }
            return false;
        }
    }
}