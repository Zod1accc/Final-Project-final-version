namespace TechnoStore.Models
{
	public class OrderItem
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public double SellPrice { get; set; }
		public double CostPrice { get; set; }
		public double DiscountPrice { get; set; }
		public int Count { get; set; }
		public Product Product { get; set; }
		public Order Order { get; set; }
	}
}
