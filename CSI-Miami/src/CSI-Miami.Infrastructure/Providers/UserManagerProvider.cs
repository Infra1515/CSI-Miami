using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSI_Miami.Data.Models;
using CSI_Miami.Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CSI_Miami.Infrastructure.Providers
{
    public class UserManagerProvider : IUserManagerProvider
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserManagerProvider(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return this.userManager.GetUserId(principal);
        }

        public string GetUserName(ClaimsPrincipal principal)
        {
            return this.userManager.GetUserName(principal);
        }

        public virtual Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }

        public virtual IQueryable<ApplicationUser> Users
        {
            get
            {
                return this.userManager.Users;
            }
        }
    }
}
