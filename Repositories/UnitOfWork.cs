using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private LearnToLearnContext context = new LearnToLearnContext();

        private CourseRepository courseRepository;
        private EnrollmentRepository enrollmentRepository;
        private UserRepository userRepository;


        public CourseRepository CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new CourseRepository(context);
                }
                return courseRepository;
            }
        }

        public EnrollmentRepository EnrollmentRepository
        {

            get
            {
                if (this.enrollmentRepository == null)
                {
                    this.enrollmentRepository = new EnrollmentRepository(context);
                }
                return enrollmentRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }


        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
