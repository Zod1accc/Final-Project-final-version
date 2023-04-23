using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Color
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:30)]
		public string Name { get; set; }
	}
}
