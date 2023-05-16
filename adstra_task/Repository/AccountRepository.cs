using adstra_task.AutoMapper;
using adstra_task.Models;
using adstra_task.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace adstra_task.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public IWebHostEnvironment Hosting { get; }


        public AccountRepository(RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, UserManager<User> userManager, IWebHostEnvironment hosting)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            Hosting = hosting;
        }
        public async Task CreateDefaultAdmin()
        {

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }
            var adminUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                Email = "admin@admin.com"
            };

            await userManager.CreateAsync(adminUser, "123");

            await userManager.AddToRoleAsync(adminUser, "Admin");

        }

        public async Task<IList<User>> GetUsers()
        {
            var usersTask = userManager.GetUsersInRoleAsync("User");
            var users = await usersTask;
            return users;
        }

        public async Task<IList<User>> GetAdmins()
        {
            var usersTask = userManager.GetUsersInRoleAsync("Admin");
            var users = await usersTask;
            return users;
        }

        public async Task Login(UserViewModel model)
        {
            await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

        }

        public void Logout()
        {
            signInManager.SignOutAsync();
        }

        public async Task Register(UserViewModel model)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            var existUser = await userManager.FindByEmailAsync(model.Email);
            if (adminUser == null)
            {
                await CreateDefaultAdmin();
            }
            var MappConfig = AutoConfigMapper.CreateMapper();
            var user = MappConfig.Map<User>(model);
            if (existUser == null)
            {
                user.Id = Guid.NewGuid().ToString();
                user.UserImage = (model.File != null) ? SaveImage(model.File) : model.UserImage;
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    CreateRoles(roleManager).Wait();
                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, isPersistent: false);
                }
            }

        }

        public string SaveImage(IFormFile file)
        {
            var ImagePath = "";

            if (file.FileName != null)
            {
                string _newPath = Path.Combine(Hosting.WebRootPath, "ImageUsers");
                FileInfo f = new FileInfo(file.FileName);

                ImagePath = Guid.NewGuid().ToString() + f.Extension;
                string FullPath = Path.Combine(_newPath, ImagePath);

                file.CopyTo(new FileStream(FullPath, FileMode.Create));

            }

            return ImagePath;
        }

        public async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (string role in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task<User> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<IList<string>> GetRole(UserViewModel user)
        {
            var userDetails = await userManager.FindByNameAsync(user.UserName);
            var roles = await userManager.GetRolesAsync(userDetails);
            return roles.ToList();
        }

    }
}
