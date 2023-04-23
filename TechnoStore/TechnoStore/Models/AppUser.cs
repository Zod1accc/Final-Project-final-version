using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [StringLength(maximumLength:50)]
        public string Fullname { get; set; }
        public bool IsBanned { get; set; }
    }
}
