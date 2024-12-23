using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CanIEatIt.Controllers
{
	public class UsersController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;

		private readonly ILogger<UsersController> _logger;

		public UsersController(UserManager<IdentityUser> userManager, ILogger<UsersController> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}
		public async Task<IActionResult> Admin(string key)
		{
			if (key != "MyKey")
			{
				return Unauthorized("You are not authorized to do that.");
			}
			// Make current user admin

			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound("User Not Found.");
			}

			if (await _userManager.IsInRoleAsync(user, "Admin"))
			{
				return BadRequest("User is already an Admin");
			}

			var result = await _userManager.AddToRoleAsync(user, "Admin");

			if (!result.Succeeded)
			{
				return StatusCode(500, "Failed to assign Admin Role.");
			}

			_logger.LogInformation($"User {user.UserName} was granted Admin priviledges.");


			return RedirectToAction("Index");
		}
	}
}
