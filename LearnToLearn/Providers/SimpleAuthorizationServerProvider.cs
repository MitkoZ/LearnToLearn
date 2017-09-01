using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Services;
using DataAccess;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using DataAccess.Models;

namespace LearnToLearn.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            UnitOfWork unitOfWork = new UnitOfWork();
            var formCollection = context.Request.ReadFormAsync().Result;
            string email = formCollection.Get("Email");
            string password = formCollection.Get("Password");
            User userDb = unitOfWork.UserRepository.GetUser(email, password);

            if (userDb == null)
            {
                context.SetError("invalid_grant", "The email or password is incorrect.");
                return base.GrantResourceOwnerCredentials(context);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);        
            identity.AddClaim(new Claim(ClaimTypes.Role, userDb.IsTeacher ? "Teacher" : "NormalUser"));
            context.Validated(identity);
            return base.GrantResourceOwnerCredentials(context);
        }
    }
}