using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CourseService : BaseService<Course, int, CourseRepository, UnitOfWork>
    {
        public CourseService(IValidationDictionary validationDictionary, CourseRepository repository, UnitOfWork unitOfWork) : base(validationDictionary, repository, unitOfWork)
        {
        }

        public bool IsCourseExist(int id)
        {
            Course course = repository.GetAll(x => x.Id == id).FirstOrDefault();
            if (course == null)
            {
                return false;
            }
            return true;
        }

        public bool IsVisible(int id)
        {
            Course course = repository.GetAll(x => x.Id == id).FirstOrDefault();
            if (course.IsVisible)
            {
                return true;
            }
            return false;
        }
    }
}
