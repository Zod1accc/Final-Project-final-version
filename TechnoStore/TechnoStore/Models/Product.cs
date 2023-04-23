using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Product
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public int BrandId { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Name { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Description { get; set; }
		public double SellCount { get; set; }
		public double SellPrice { get; set; }
		public double DiscountPrice { get; set; }
		public double CostPrice { get; set; }
		public int ProdutCount { get; set; }
		public bool IsAvailablity { get; set; }
		public bool IsFeatured { get; set; }
		public bool IsNew { get; set; }
		public int Viewed { get; set; }
		public Category Category { get; set; }
		public Brand Brand { get; set; }
		public List<ProductImage> Images { get; set; }

		

	}
}
