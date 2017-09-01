using DataAccess;
using DataAccess.Models;
using LearnToLearn.BindModels.User;
using Microsoft.AspNet.Identity;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LearnToLearn.Controllers
{
    public class UserController : ApiController
    {
        #region Constructors and fields
        private UserService userService;
        private ModelStateWrapper modelStateWrapper;
        public UserController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            modelStateWrapper = new ModelStateWrapper(this.ModelState);
            userService = new UserService(modelStateWrapper, unitOfWork.UserRepository, unitOfWork);
        }
        #endregion

        [HttpPost]
        public IHttpActionResult Register(CreateUserBindModel createUserBindModel)
        {
            if (!userService.PreValidate())
            {
                return BadRequest(this.modelStateWrapper.ModelState);
            }
            if (userService.IsUserExist(x => x.Email == createUserBindModel.Email))
            {
                return new CustomHttpErrorResult(Request, HttpStatusCode.Conflict, "The current email is already used!");
            }
            if (userService.RegisterUser(createUserBindModel.Email, createUserBindModel.Name, createUserBindModel.Password))
            {
                return Ok();
            }
            else
            {
                return BadRequest(this.modelStateWrapper.ModelState);
            }
        }
    }
}
