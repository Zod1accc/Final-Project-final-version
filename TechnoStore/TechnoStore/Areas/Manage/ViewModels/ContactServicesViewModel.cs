namespace TechnoStore.Areas.Manage.ViewModels
{
	public class ContactServicesViewModel
	{
		public int Id { get; set; }
		public string? IconStr { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public IFormFile? Icon { get; set; }
	}
}
