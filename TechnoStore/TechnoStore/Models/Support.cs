using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Support
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:200)]
		public string Title { get; set; }
		[Required]
		[StringLength(maximumLength:2000)]
		public string Description { get; set; }
	}
}
