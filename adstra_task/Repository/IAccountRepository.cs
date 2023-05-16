using adstra_task.Models;
using adstra_task.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace adstra_task.Repository
{

    public interface IAccountRepository
    {
        Task Login(UserViewModel model);
        Task Register(UserViewModel model);
        void Logout();
        Task CreateRoles(RoleManager<IdentityRole> roleManager);
        Task<IList<User>> GetUsers();
        Task<IList<User>> GetAdmins();
        Task<User> GetUser(string id);
        string SaveImage(IFormFile file);
        Task<IList<string>> GetRole(UserViewModel user);
    }
}
