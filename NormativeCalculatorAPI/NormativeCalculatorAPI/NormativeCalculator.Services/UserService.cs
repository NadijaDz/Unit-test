using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace NormativeCalculator.Services
{
    public interface IUserService
    {
        Task<IdentityUser> GetLoggedInUser();
    }

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityUser> GetLoggedInUser()
        {
            var getUser = _httpContextAccessor.HttpContext.User.Identity;
            var user = await _userManager.FindByNameAsync(getUser.Name);

            return user;
        }
    }
}
