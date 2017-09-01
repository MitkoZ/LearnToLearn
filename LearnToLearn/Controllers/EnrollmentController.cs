using DataAccess.Models;
using LearnToLearn.BindModels;
using LearnToLearn.BindModels.Enrollment;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LearnToLearn.Controllers
{
    public class EnrollmentController : ApiController
    {
        #region Constructors and fields
        private EnrollmentService enrollmentService;
        private CourseService courseService;
        private UserService userService;
        private ModelStateWrapper modelStateWrapper;
        public EnrollmentController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            modelStateWrapper = new ModelStateWrapper(this.ModelState);
            enrollmentService = new EnrollmentService(modelStateWrapper, unitOfWork.EnrollmentRepository, unitOfWork);
            courseService = new CourseService(modelStateWrapper, unitOfWork.CourseRepository, unitOfWork);
            userService = new UserService(modelStateWrapper, unitOfWork.UserRepository, unitOfWork);
        }
        #endregion

        [Authorize(Roles = "NormalUser")]
        [HttpPost]
        public IHttpActionResult Create(CreateEnrollmentBindModel createEnrollmentBindModel)
        {
            if (!enrollmentService.PreValidate())
            {
                return BadRequest(modelStateWrapper.ModelState);
            }
            if (!userService.IsUserExist(createEnrollmentBindModel.UserId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "User with this UserId not found!");
            }
            if (userService.IsTeacher(createEnrollmentBindModel.UserId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "A teacher cannot enroll in a course!");
            }
            if (!courseService.IsCourseExist(createEnrollmentBindModel.CourseId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Course not found!");
            }
            if (enrollmentService.IsUserAlreadyEnrolled(createEnrollmentBindModel.CourseId, createEnrollmentBindModel.UserId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.BadRequest, "You have already enrolled in this course!");
            }
            if (!enrollmentService.IsEmptySlot(createEnrollmentBindModel.CourseId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Forbidden, "The current course's capacity is full!");
            }
            Enrollment enrollment = new Enrollment();
            enrollment.CourseId = createEnrollmentBindModel.CourseId;
            enrollment.UserId = createEnrollmentBindModel.UserId;
            enrollment.CreatedAt = DateTime.Now;
            enrollment.UpdatedAt = DateTime.Now;
            if (enrollmentService.Save(enrollment))
            {
                ShowEnrollmentBindModel showEnrollmentBindModel = new ShowEnrollmentBindModel();
                showEnrollmentBindModel.Id = enrollment.Id;
                showEnrollmentBindModel.Grade = enrollment.Grade;
                showEnrollmentBindModel.UserId = enrollment.UserId;
                showEnrollmentBindModel.CourseId = enrollment.CourseId;
                showEnrollmentBindModel.CreatedAt = enrollment.CreatedAt;
                showEnrollmentBindModel.UpdatedAt = enrollment.UpdatedAt;
                return Ok(showEnrollmentBindModel);
            }

            return StatusCode(HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch]
        public IHttpActionResult Grade(GradeBindModel gradeBindModel)
        {
            if (!enrollmentService.PreValidate())
            {
                return BadRequest(modelStateWrapper.ModelState);
            }
            if (!userService.IsTeacher(gradeBindModel.TeacherId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are not a teacher!");
            }
            if (!courseService.IsCourseExist(gradeBindModel.CourseId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Course not found!");
            }
            if (!userService.IsAuthorized(gradeBindModel.TeacherId, gradeBindModel.CourseId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Unauthorized, "You are a teacher but this course doesn't belong to you!");
            }
            if (!userService.IsUserExist(gradeBindModel.StudentId))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.NotFound, "Student not found!");
            }
            Enrollment enrollment = enrollmentService.GetAll(x => x.CourseId == gradeBindModel.CourseId && x.UserId == gradeBindModel.StudentId).FirstOrDefault();
            enrollment.Grade = gradeBindModel.Grade;
            enrollment.UpdatedAt = DateTime.Now;
            if (enrollmentService.Save(enrollment))
            {
                ShowEnrollmentBindModel showEnrollmentBindModel = new ShowEnrollmentBindModel();
                showEnrollmentBindModel.Id = enrollment.Id;
                showEnrollmentBindModel.Grade = enrollment.Grade;
                showEnrollmentBindModel.UserId = enrollment.UserId;
                showEnrollmentBindModel.CourseId = enrollment.CourseId;
                showEnrollmentBindModel.CreatedAt = enrollment.CreatedAt;
                showEnrollmentBindModel.UpdatedAt = enrollment.UpdatedAt;
                return Ok(showEnrollmentBindModel);
            }
            return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}
