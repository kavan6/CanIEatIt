using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CanIEatIt.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ILogger<AccountController> _logger;

		private readonly List<string> _adminValidationKeys;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, List<string> adminValidationKeys, ILogger<AccountController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_adminValidationKeys = adminValidationKeys;
		}
		public IActionResult AccessDenied()
		{
			return RedirectToAction("Index", "Home");
		}
		private async Task<bool> ValidateKeyAsync(string key)
		{
			return _adminValidationKeys.Contains(key);
		}
		public async Task<IActionResult> Admin([FromQuery] string key)
		{

			if (string.IsNullOrEmpty(key) || !await ValidateKeyAsync(key))
			{
				return Unauthorized("You are not authorized to do that");
			}

			var user = await _userManager.FindByNameAsync("Admin");

			if (user == null)
			{
				return Unauthorized("Admin user not found");
			}

			await _signInManager.SignInAsync(user, isPersistent: false);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, "Admin")
			};

			var identity = new ClaimsIdentity(claims, "DefaultScheme");

			var principle = new ClaimsPrincipal(identity);

			HttpContext.User = principle;

			await _signInManager.SignInAsync(user, isPersistent: false);

			return RedirectToAction("Index", "Home");
		}

        public async Task<IActionResult> Basic()
		{

			var user = await _userManager.FindByNameAsync("BasicUser");

			if (user == null)
			{
				return Unauthorized("Basic user not found");
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, "BasicUser")
			};

			var identity = new ClaimsIdentity(claims, "DefaultScheme");

			var principle = new ClaimsPrincipal(identity);

			HttpContext.User = principle;

			await _signInManager.SignInAsync(user, isPersistent: false);

			return RedirectToAction("Index", "Home");
		}

        public async Task<IActionResult> Logout()
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized("User is not logged in.");
			}

			await _signInManager.SignOutAsync();

			return RedirectToAction("Basic");
		}
	}
}
