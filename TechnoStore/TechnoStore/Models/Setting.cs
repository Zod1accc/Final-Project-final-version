using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class Setting
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Callus { get; set; }
		[Required]
		[StringLength(maximumLength: 80)]
		public string CallusText { get; set; }
		[Required]
		[StringLength(maximumLength:100)]
		public string Address { get; set; }
		public string CallusIcon { get; set; }
		public string Logo { get; set; }
		public string FacebookLink { get; set; }
		public string TwitterLink { get; set; }
		public string Instagram { get; set; }
		public string Google { get; set; }


	}
}
