using TechnoStore.Models;

namespace TechnoStore.ViewModels
{
	public class ProductDetailViewModel
	{
		public Product Product { get; set; }
		public List<Product> RelatedProduct { get; set; }
		public Dictionary<string,string> keyValuePairs { get; set; }
	}
}
