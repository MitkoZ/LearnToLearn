﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Course : BaseEntity, IBaseEntity<int>
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey("User")]
        public string TeacherId { get; set; }

        [Required]
        public bool IsVisible { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }

        public virtual User User { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }
        public Course()
        {
            Enrollments = new List<Enrollment>();
        }
    }
}
