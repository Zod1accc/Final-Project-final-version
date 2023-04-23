using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class ContactService
	{
		public int Id { get; set; }
		public string Icon { get; set; }
		[Required]
		[StringLength(maximumLength:100)]
		public string Title { get; set; }
		[Required]
		[StringLength(maximumLength:200)]
		public string Content { get; set; }
	}
}
