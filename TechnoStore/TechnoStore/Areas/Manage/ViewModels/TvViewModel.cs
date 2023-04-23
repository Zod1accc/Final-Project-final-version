using System.ComponentModel.DataAnnotations;
using TechnoStore.Models;

namespace TechnoStore.Areas.Manage.ViewModels
{
    public class TvViewModel
    {
        public int? Id { get; set; }
        public int BrandId { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Description { get; set; }
        public double SellPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double CostPrice { get; set; }
        public int ProductCount { get; set; }
        public bool IsAvailablity { get; set; }
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        public List<ProductImage>? Images { get; set; }
        public IFormFile? PosterImage { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public List<int>? ImageIds { get; set; }


		[StringLength(maximumLength: 50)]
		public string? EkranNovu { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EkranIcazesi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? Tezlik { get; set; }
		[StringLength(maximumLength: 50)]
		public string? SesSistemi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? IşığınNövü { get; set; }
		public double? Cheki { get; set; }
		[StringLength(maximumLength: 50)]
		public string? Olchu { get; set; }
		[StringLength(maximumLength: 50)]
		public string? İstehsalcı { get; set; }
	}
}
