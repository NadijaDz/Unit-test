using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NormativeCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider.
            //string redirectUrl = "https://localhost:5001/api/Login/external-callback?returnUrl=" + returnUrl;
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Login", new { returnUrl });

            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }


        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {

            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null) return new RedirectResult($"{returnUrl}?error=externalsigninerror");

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return new RedirectResult($"{returnUrl}?error=lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.

                string loginprovider = info.LoginProvider;
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                string name = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                string surname = info.Principal.FindFirstValue(ClaimTypes.Surname);

                var user = new IdentityUser
                {
                    Email = email,
                    UserName = name

                };
                var resultCreateUser = await _userManager.CreateAsync(user);
                if (resultCreateUser.Succeeded)
                {
                    resultCreateUser = await _userManager.AddLoginAsync(user, info);
                    if (resultCreateUser.Succeeded)
                    {

                        return Redirect(returnUrl);
                    }
                }
                return Unauthorized();
            }
        }

        [HttpGet("signout-google")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
