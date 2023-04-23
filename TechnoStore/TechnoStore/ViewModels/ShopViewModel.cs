using TechnoStore.Helpers;
using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
	public class ShopViewModel
	{
		public List<Product> Products { get; set; }
		public List<Product> BestSeller { get; set; }
		public List<Product> IsFeatured { get; set; }
		public List<Product> IsNew { get; set; }
		public PaginatedList<Product> PaginatedProducts { get; set;}

	}
}
