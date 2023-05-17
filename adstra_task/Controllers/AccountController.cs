using adstra_task.Models;
using adstra_task.Repository;
using adstra_task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace adstra_task.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAccountRepository Account;
        private readonly UserManager<User> userManager;

        public AccountController(IAccountRepository account, UserManager<User> userManager)
        {
            Account = account;
            this.userManager = userManager;
        }


        [AllowAnonymous]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Register(UserViewModel model)
        {

            await Account.Register(model);
            return RedirectToAction("Dashboard", "Home");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserViewModel user)
        {

            await Account.Login(user);
            var roles = await Account.GetRole(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }


        }
        [HttpPost]
        public IActionResult Logout()
        {
            Account.Logout();
            return RedirectToAction(nameof(Login));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveAdminRole(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(user, "User");
            }
            return RedirectToAction("Dashboard", "Home");
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> AddToAdminRole(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, "User");
                await userManager.AddToRoleAsync(user, "Admin");
            }
            return RedirectToAction("Dashboard", "Home");
        }


    }
}
