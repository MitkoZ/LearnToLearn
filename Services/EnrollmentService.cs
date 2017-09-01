using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EnrollmentService : BaseService<Enrollment, int, EnrollmentRepository, UnitOfWork>
    {
        public EnrollmentService(IValidationDictionary validationDictionary, EnrollmentRepository repository, UnitOfWork unitOfWork) : base(validationDictionary, repository, unitOfWork)
        {
        }

        public bool IsEmptySlot(int courseId)
        {
            int courseCapacity = unitOfWork.CourseRepository.GetAll(x => x.Id == courseId).FirstOrDefault().Capacity;
            int currentTakenSlots = repository.GetAll(x => x.CourseId == courseId).Count;
            if (currentTakenSlots == courseCapacity)
            {
                return false;
            }
            return true;
        }

        public bool IsUserAlreadyEnrolled(int courseId, string userId)
        {
            Enrollment enrollmentDb = repository.GetAll(x => x.CourseId == courseId && x.UserId == userId).FirstOrDefault();
            if (enrollmentDb != null)
            {
                return true;
            }
            return false;
        }
    }
}
