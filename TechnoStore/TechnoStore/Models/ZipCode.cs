using System.ComponentModel.DataAnnotations;

namespace TechnoStore.Models
{
	public class ZipCode
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength:50)]
		public string Code { get; set; }
		public DateTime CreatedDate { get; set; }
		public int ValidityPeriodInDays { get; set; }
		public double DiscountPrice { get; set; }
		public bool IsDisabled { get { return DateTime.Now > CreatedDate.AddDays(ValidityPeriodInDays); } }
	}
}
