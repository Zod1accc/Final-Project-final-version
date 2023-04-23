
using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Name { get; set; }
	}
}
