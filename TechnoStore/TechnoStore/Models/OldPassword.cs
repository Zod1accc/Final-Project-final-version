namespace TechnoStore.Models
{
	public class OldPassword
	{
		public int Id { get; set; }
		public string AppUserId { get; set; }
		public string Password { get; set; }
		public AppUser AppUser { get; set; }

	}
}
