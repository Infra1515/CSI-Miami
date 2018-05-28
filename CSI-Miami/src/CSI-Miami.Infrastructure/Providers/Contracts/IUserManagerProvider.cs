using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSI_Miami.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CSI_Miami.Infrastructure.Providers.Contracts
{
    public interface IUserManagerProvider
    {

        string GetUserId(ClaimsPrincipal principal);
        string GetUserName(ClaimsPrincipal principal);
        IQueryable<ApplicationUser> Users { get; }
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

    }
}
