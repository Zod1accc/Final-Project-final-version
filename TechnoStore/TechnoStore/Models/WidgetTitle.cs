using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class WidgetTitle
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:50)]
		public string Title { get; set; }
	}
}
