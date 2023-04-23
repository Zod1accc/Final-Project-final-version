using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Service
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:30)]
		public string Title { get; set; }
		[Required]
		[StringLength(maximumLength: 60)]
		public string Description { get; set; }
		public string Image { get; set; }
	}
}
