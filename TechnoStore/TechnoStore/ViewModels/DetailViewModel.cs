using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
    public class DetailViewModel
    {
        public List<Order>? Orders { get;set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
        public bool IsBanned { get; set; }
        public string RoleId { get; set; }
    }
}
