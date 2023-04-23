using TechnoStore.ViewModels.Mailsender;

namespace TechnoStore.Abstractions.MailService
{
	public interface IMailService
	{
		Task SendEmailAsync(MailRequestVM mailRequest);
	}
}
