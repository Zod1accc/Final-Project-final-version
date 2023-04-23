using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class WidgetItem
	{
		public int Id { get; set; }
		public int WidgetTitleId { get; set; }
		[Required]
		[StringLength(maximumLength:50)]
		public string Name { get; set; }
		public string RedirectUrl { get; set; }
		public WidgetTitle? WidgetTitle { get; set; }
	}
}
