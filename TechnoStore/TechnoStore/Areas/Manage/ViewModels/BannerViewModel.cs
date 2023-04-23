namespace TechnoStore.Areas.Manage.ViewModels
{
    public class BannerViewModel
    {
        public int? Id { get; set; }
        public string RedirectUrl { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
