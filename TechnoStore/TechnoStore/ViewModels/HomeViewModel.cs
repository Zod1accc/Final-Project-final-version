using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Service> Services { get; set; }
        public List<Product> IsFeatured { get; set; }
        public List<Product> IsNew { get; set; }
        public List<Product> DiscountProduct { get; set; }
        public List<Product> Smartphones { get; set; }
        public List<Product> Tvs { get; set; }
        public List<Product> OurProducts { get; set; }
        public List<HomePageBanner> HomePageBanners { get; set; }
        public List<Product> BestSeller { get; set; }
        public List<Product> MostViewed { get; set; }

    }
}
