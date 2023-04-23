using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Country
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:60)]
		public string Name { get; set; }
	}
}
