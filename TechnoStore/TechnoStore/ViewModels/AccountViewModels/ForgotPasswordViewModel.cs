using System.ComponentModel.DataAnnotations;

namespace TechnoStore.ViewModels.AccountViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required,StringLength(maximumLength:256),DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	}
}
