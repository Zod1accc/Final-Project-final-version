namespace TechnoStore.Models
{
	public class BasketItem
	{
		public int Id { get; set; }
		public string AppUserId { get; set; }
		public int ProductId { get; set; }
		public bool IsDeleted { get; set; }
		public int Count { get; set; }
		public Product Product { get; set; }
		public AppUser AppUser { get; set; }
	}
}
