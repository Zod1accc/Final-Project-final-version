using System.ComponentModel.DataAnnotations;
using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
	public class ContactViewModel
	{

		[Required]
		[StringLength(maximumLength: 255)]
		public string Fullname { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		public string Subject { get; set; }
		[Required]
		public string Comment { get; set; }
		public List<ContactCenter>? contactCenters { get; set; }
		public List<ContactService>? contactServices { get; set; }
	}
}
