using Microsoft.Extensions.Options;
using TaskUltimate.ViewModel;

namespace TaskUltimate.Interfaces
{
    public interface ISendMessageService
    {
        public Task SendEmailMessage(MailRequest mailRequest);
        public void SendWhatsAppMessage(string To, string Message);
    }
}
