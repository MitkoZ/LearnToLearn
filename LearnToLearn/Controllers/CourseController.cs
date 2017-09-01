using DataAccess.Models;
using LearnToLearn.BindModels.Course;
using LearnToLearn.ViewModels;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace LearnToLearn.Controllers
{
    public class CourseController : ApiController
    {
        #region Constructors and fields
        private CourseService courseService;
        private UserService userService;
        private EnrollmentService enrollmentService;
        private ModelStateWrapper modelStateWrapper;
        public CourseController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            modelStateWrapper = new ModelStateWrapper(this.ModelState);
            courseService = new CourseService(modelStateWrapper, unitOfWork.CourseRepository, unitOfWork);
            userService = new UserService(modelStateWrapper, unitOfWork.UserRepository, unitOfWork);
            enrollmentService = new EnrollmentService(modelStateWrapper, unitOfWork.EnrollmentRepository, unitOfWork);
        }
        #endregion

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IHttpActionResult Create(CreateCourseBindModel createCourseBindModel)
        {
            if (!courseService.PreValidate())
            {
                return BadRequest(modelStateWrapper.ModelState);
            }
            if (!userService.IsUserExist(createCourseBindModel.TeacherId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "User with this UserId not found!");
            }
            if (!userService.IsTeacher(createCourseBindModel.TeacherId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are not a teacher!");
            }
            Course courseDb = new Course();
            courseDb.TeacherId = createCourseBindModel.TeacherId;
            courseDb.IsVisible = true;
            courseDb.Name = createCourseBindModel.Name;
            courseDb.Capacity = createCourseBindModel.Capacity;
            courseDb.CreatedAt = DateTime.Now;
            courseDb.UpdatedAt = DateTime.Now;
            courseDb.Description = createCourseBindModel.Description;

            if (courseService.Save(courseDb))
            {
                ShowCourseBindModel showCourseBindModel = new ShowCourseBindModel();
                showCourseBindModel.Id = courseDb.Id;
                showCourseBindModel.Name = courseDb.Name;
                showCourseBindModel.Capacity = courseDb.Capacity;
                showCourseBindModel.Description = courseDb.Description;
                showCourseBindModel.IsVisible = courseDb.IsVisible;
                showCourseBindModel.CreatedAt = courseDb.CreatedAt;
                showCourseBindModel.UpdatedAt = courseDb.UpdatedAt;
                showCourseBindModel.TeacherId = courseDb.TeacherId;
                showCourseBindModel.CreatedAt = courseDb.CreatedAt;
                showCourseBindModel.UpdatedAt = courseDb.UpdatedAt;
                return Ok(showCourseBindModel);
            }
            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Teacher, NormalUser")]
        [HttpGet]
        public List<ShowCourseBindModel> Get()
        {
            List<ShowCourseBindModel> showCoursesBindModel = new List<ShowCourseBindModel>();
            foreach (Course course in courseService.GetAll(x => x.IsVisible))
            {
                showCoursesBindModel.Add(new ShowCourseBindModel()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Capacity = course.Capacity,
                    Description = course.Description,
                    IsVisible = course.IsVisible,
                    CreatedAt = course.CreatedAt,
                    UpdatedAt = course.UpdatedAt,
                    TeacherId = course.TeacherId
                });
            }
            return showCoursesBindModel;
        }

        [Authorize(Roles = "Teacher, NormalUser")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (!courseService.IsCourseExist(id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Course not found!");
            }
            if (!courseService.IsVisible(id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Forbidden, "The course that you are trying to get isn't visible anymore!");
            }
            Course courseDb = courseService.GetAll(x => x.Id == id).FirstOrDefault();
            ShowCourseBindModel showCourseBindModel = new ShowCourseBindModel()
            {
                Id = courseDb.Id,
                Name = courseDb.Name,
                Capacity = courseDb.Capacity,
                Description = courseDb.Description,
                IsVisible = courseDb.IsVisible,
                CreatedAt = courseDb.CreatedAt,
                UpdatedAt = courseDb.UpdatedAt,
                TeacherId = courseDb.TeacherId
            };
            return Ok(showCourseBindModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPut]
        public IHttpActionResult Edit(int id, EditCourseBindModel editCourseBindModel)
        {
            if (!courseService.PreValidate())
            {
                return BadRequest(modelStateWrapper.ModelState);
            }
            if (!userService.IsTeacher(editCourseBindModel.TeacherId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are not a teacher!");
            }
            if (!userService.IsAuthorized(editCourseBindModel.TeacherId, id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are a teacher but this course doesn't belong to you!");
            }
            if (!courseService.IsCourseExist(id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Course not found!");
            }
            Course courseDb = courseService.GetAll(x => x.Id == id).FirstOrDefault();
            courseDb.Name = editCourseBindModel.Name;
            courseDb.IsVisible = editCourseBindModel.IsVisible;
            courseDb.Capacity = editCourseBindModel.Capacity;
            courseDb.Description = editCourseBindModel.Description;
            courseDb.UpdatedAt = DateTime.Now;
            if (courseService.Save(courseDb))
            {
                ShowCourseBindModel showCourseBindModel = new ShowCourseBindModel();
                showCourseBindModel.Id = courseDb.Id;
                showCourseBindModel.Name = courseDb.Name;
                showCourseBindModel.Capacity = courseDb.Capacity;
                showCourseBindModel.Description = courseDb.Description;
                showCourseBindModel.IsVisible = courseDb.IsVisible;
                showCourseBindModel.CreatedAt = courseDb.CreatedAt;
                showCourseBindModel.UpdatedAt = courseDb.UpdatedAt;
                showCourseBindModel.TeacherId = courseDb.TeacherId;
                return Ok(showCourseBindModel);
            }
            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete]
        public IHttpActionResult Delete(int id, DeleteCourseBindModel deleteCourseBindModel)
        {
            if (!userService.PreValidate())
            {
                return BadRequest(modelStateWrapper.ModelState);
            }
            if (!userService.IsTeacher(deleteCourseBindModel.TeacherId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are not a teacher!");
            }
            if (!courseService.IsCourseExist(id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Course not found!");
            }
            if (!userService.IsAuthorized(deleteCourseBindModel.TeacherId, id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are a teacher but this course doesn't belong to you!");
            }
            if (enrollmentService.DeleteByPredicate(x => x.CourseId == id)) //manual cascade delete
            {
                if (courseService.DeleteById(id))
                {
                    return Ok();
                }
            }

            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch]
        public IHttpActionResult MarkIsVisible(int id, MarkIsVisibleBindModel markIsVisibleBindModel)
        {
            if (!courseService.PreValidate())
            {
                return BadRequest(modelStateWrapper.ModelState);
            }
            if (!userService.IsTeacher(markIsVisibleBindModel.TeacherId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are not a teacher!");
            }
            if (!courseService.IsCourseExist(id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Course not found!");
            }
            if (!userService.IsAuthorized(markIsVisibleBindModel.TeacherId, id))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are a teacher but this course doesn't belong to you!");
            }
            Course courseDb = courseService.GetAll(x => x.Id == id).FirstOrDefault();
            courseDb.IsVisible = markIsVisibleBindModel.IsVisible;
            if (courseService.Save(courseDb))
            {
                ShowCourseBindModel showCourseBindModel = new ShowCourseBindModel()
                {
                    Id = courseDb.Id,
                    Name = courseDb.Name,
                    Capacity = courseDb.Capacity,
                    CreatedAt = courseDb.CreatedAt,
                    UpdatedAt = courseDb.UpdatedAt,
                    Description = courseDb.Description,
                    IsVisible = courseDb.IsVisible,
                    TeacherId = courseDb.TeacherId
                };
                return Ok(showCourseBindModel);
            }
            return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}
