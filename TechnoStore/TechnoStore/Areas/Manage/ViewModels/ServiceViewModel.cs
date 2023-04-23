using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Areas.Manage.ViewModels
{
	public class ServiceViewModel
	{
		public int? Id { get; set; }
		[Required]
		[StringLength(maximumLength: 30)]
		public string Title { get; set; }
		[Required]
		[StringLength(maximumLength: 60)]
		public string Description { get; set; }
		public string? Image { get; set; }
		public IFormFile? ImageFile { get; set; }
	}
}
