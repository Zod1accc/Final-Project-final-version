using System.ComponentModel.DataAnnotations;
using TechnoStore.Models;

namespace TechnoStore.Areas.Manage.ViewModels
{
	public class PhoneViewModel
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
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public List<ProductImage>? Images { get; set; }
		public IFormFile? PosterImage { get; set; }
		public List<IFormFile>? ImageFiles { get; set; }
		public List<int>? ImageIds { get; set; }
		//Features
		[StringLength(maximumLength: 50)]
		public string? Ekran { get; set; }
		[StringLength(maximumLength: 50)]
		public string? DaxiliYaddaş { get; set; }
		[StringLength(maximumLength: 50)]
		public string? OperativYaddaş { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EsasKamera { get; set; }
		[StringLength(maximumLength: 50)]
		public string? OnKamera { get; set; }
		public int? NüveSayı { get; set; }
		[StringLength(maximumLength: 50)]
		public string? ProsessorunAdı { get; set; }
		[StringLength(maximumLength: 50)]
		public string? ProsessorunTezliyi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EmeliyyatSistemi { get; set; }
		[StringLength(maximumLength: 50)]
		public string? EmeliyyatSistemiVersiyası { get; set; }
		public int? İstehsalİli { get; set; }
		public double? Çeki { get; set; }
		[StringLength(maximumLength: 50)]
		public string? İstehsalçı { get; set; }
	}
}
