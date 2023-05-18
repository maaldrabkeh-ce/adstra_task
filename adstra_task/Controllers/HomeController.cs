using adstra_task.Models;
using adstra_task.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace adstra_task.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository Account;

        public HomeController(ILogger<HomeController> logger, IAccountRepository account)
        {
            _logger = logger;
            Account = account;
        }
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            var data = await Account.GetUser(id);
            return View(data);
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var data = await Account.GetUsers();
            return View(data);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            var data = await Account.GetAdmins();
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
