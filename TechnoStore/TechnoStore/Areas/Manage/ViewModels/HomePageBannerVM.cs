namespace TechnoStore.Areas.Manage.ViewModels
{
	public class HomePageBannerVM
	{
		public int? Id { get; set; }
		public string? Image { get; set; }
		public string? Url { get; set; }
		public string? Name { get; set; }
		public bool Disable { get; set; }
		public IFormFile ImageFile { get; set; }
	}
}
