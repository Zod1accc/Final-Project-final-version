using System.ComponentModel.DataAnnotations;
using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
	public class OrderViewModel
	{
		public List<CheckOutViewModel>? checkoutItemViewModels { get; set; }

		public List<Service>? services { get; set; }
		public string Fullname { get; set; }
		public int CountryId { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		[Required]
		public string Address1 { get; set; }
		public string? Address2 { get; set; }
		public string City { get; set; }
		public string? ZipCode { get; set; }
		public string? Note { get; set; }
	}
}
