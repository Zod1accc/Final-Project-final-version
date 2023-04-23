using TechnoStore.Helpers;
using TechnoStore.Models;

namespace TechnoStore.Areas.Manage.ViewModels
{
	public class DashboardViewModel
	{
		public List<Product> Products { get; set; }
		public List<Order> Orders { get; set; }
		public List<AppUser> Users { get; set; }
		public List<Product> BestSellers { get; set; }
		public List<Product> MostViewed { get; set; }
		public double Price { get; set; }
		public PaginatedList<Product> PaginatedProduct { get; set; }
		public PaginatedList<Order> PaginatedOrder { get; set; }
		public PaginatedList<Product> PaginatedBestseller { get; set; }
		public PaginatedList<Product> PaginatedMostView { get; set; }
	}
}
