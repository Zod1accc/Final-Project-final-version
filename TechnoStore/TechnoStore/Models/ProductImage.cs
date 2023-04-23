namespace TechnoStore.Models
{
	public class ProductImage
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string Image { get; set; }
		public bool IsPoster { get; set; }
		public Product Product { get; set; }
	}
}
