using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Areas.Manage.ViewModels
{
	public class SliderViewModel
	{
		public int? Id { get; set; }
		[Required]
		[StringLength(maximumLength: 40)]
		public string Header { get; set; }
		[Required]
		[StringLength(maximumLength: 20)]
		public string Name1 { get; set; }

		[StringLength(maximumLength: 20)]
		public string? Name2 { get; set; }
		public string? Image { get; set; }
		[Required]
		[StringLength(maximumLength: 250)]
		public string Description { get; set; }
		public double SellPrice { get; set; }
		public double DiscountPrice { get; set; }
		public double CostPrice { get; set; }
		public string RedirectUrl { get; set; }
		[Required]
		[StringLength(maximumLength: 20)]
		public string RedirectUrlText { get; set; }
		public IFormFile? ImageFile { get; set; }
	}
}
