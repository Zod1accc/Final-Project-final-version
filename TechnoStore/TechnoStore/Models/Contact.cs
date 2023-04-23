using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Contact
	{
		public int Id { get; set; }
		public string? AppUserId { get; set; }
		[Required]
		[StringLength(maximumLength: 255)]
		public string Fullname { get; set; }
		public string Email { get; set; }
		public DateTime CreateDate { get; set; }
		public bool Viewed { get; set; }
		[Required]
		public string Subject { get; set; }
		[Required]
		public string Comment { get; set; }
		public AppUser? AppUser { get; set; }
	}
}
