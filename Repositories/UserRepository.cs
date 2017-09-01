using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Repositories
{
    public class UserRepository : BaseRepository<User, string>
    {
        #region Constructors and fields
        private UserManager<User> userManager;
        public UserRepository(LearnToLearnContext context) : base(context)
        {
            UserStore<User> userStore = new UserStore<User>(context)
            {
                AutoSaveChanges = false
            };
            this.userManager = new UserManager<User>(userStore);
        }
        #endregion

        public IdentityResult RegisterUser(string email, string name, string password)
        {
            User user = new User
            {
                UserName = name,
                Name = name,
                Email = email
            };
            IdentityResult identityResult = userManager.Create(user, password);
            if (identityResult.Succeeded)
            {
                context.SaveChanges();
                userManager.AddToRole(user.Id, "NormalUser");
                context.SaveChanges();
            }
            return identityResult;
        }

        public User GetUser(string email, string password)
        {
            User userDb = userManager.FindByEmail(email);
            if (userDb == null)
            {
                return null;
            }
            PasswordVerificationResult passwordVerificationResult = userManager.PasswordHasher.VerifyHashedPassword(userDb.PasswordHash, password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return userDb;
        }
    }
}
