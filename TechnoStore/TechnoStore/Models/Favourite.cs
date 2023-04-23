namespace TechnoStore.Models
{
	public class Favourite
	{
		public int Id{ get; set; }
		public string AppUserId { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public AppUser MyProperty { get; set; }
	}
}
