using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
	public class SoppingCardVM
	{
		public List<Service> Services { get; set; }
		public int ItemCount { get; set; }
		public List<CheckOutViewModel> BasketItems { get; set; }
	}
}
