using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : BaseService<User, string, UserRepository, UnitOfWork>
    {
        public UserService(IValidationDictionary validationDictionary, UserRepository repository, UnitOfWork unitOfWork) : base(validationDictionary, repository, unitOfWork)
        {
        }

        public bool IsTeacher(string id)
        {
            User userDb = repository.GetAll(x => x.Id == id).FirstOrDefault();
            if (userDb.IsTeacher)
            {
                return true;
            }
            return false;
        }

        public bool IsAuthorized(string teacherId, int courseId)
        {
            bool isAuthorized = repository.GetAll(x => x.Id == teacherId).FirstOrDefault().Courses.Any(y => y.Id == courseId);
            return isAuthorized;

            //If this method returns true the current teacher is authorized to CRUD. If it returns false it means that the current user is a teacher but this course doesn't belong to him, so he can't CRUD it.
            //PS: It has to be used with an IsTeacher method before that.
        }

        public bool IsUserExist(string id)
        {
            User userDb = repository.GetAll(x => x.Id == id).FirstOrDefault();
            if (userDb == null)
            {
                return false;
            }
            return true;
        }

        public bool IsUserExist(Func<User, bool> filter)
        {
            User userDb = repository.GetAll(filter).FirstOrDefault();
            if (userDb == null)
            {
                return false;
            }
            return true;
        }

        public bool RegisterUser(string email, string name, string password)
        {
            var result = repository.RegisterUser(email, name, password);
            if (result.Succeeded)
            {
                return true;
            }
            foreach (string error in result.Errors)
            {
                this.validationDictionary.AddError(error, error);
            }
            return false;
        }

    }
}
