namespace TechnoStore.ViewModels
{
	public class BasketPartialViewModel
	{
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		public int Count { get; set; }
		public double SellPrice { get; set; }
		public double DiscountPrice { get; set; }
	}
}
