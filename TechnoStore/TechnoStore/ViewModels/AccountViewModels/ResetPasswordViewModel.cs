using System.ComponentModel.DataAnnotations;

namespace TechnoStore.ViewModels.AccountViewModels
{
	public class ResetPasswordViewModel
	{
		[Required,MaxLength(30),DataType(DataType.Password)]
		public string Password { get; set; }
		[Required, MaxLength(30), DataType(DataType.Password),Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }
	}
}
