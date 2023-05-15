using Microsoft.AspNetCore.Identity;

namespace adstra_task.Models
{
    public class User : IdentityUser
    {
        public string UserImage { get; set; }
        public string Discriminator { get; set; }
    }
}
