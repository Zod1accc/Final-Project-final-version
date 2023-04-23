using Microsoft.AspNetCore.Identity;
using TechnoStore.Models;

namespace TechnoStore.Areas.Manage.ViewModels
{
    public class UserDetailViewModel
    {
        public AppUser User { get; set; }
        public string Role { get; set; }
    }
}
