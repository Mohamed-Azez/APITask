using AutoMapper.Internal;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TaskUltimate.Interfaces;
using TaskUltimate.ViewModel;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TaskUltimate.Services
{
    public class SendMessageService : ISendMessageService
    {
        private readonly MailSettings _mailSettings;
        public SendMessageService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailMessage(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, true);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public void SendWhatsAppMessage(string To,string Message)
        {
            string accountSid = "ACd6d5fd9ab4f6379360d535092a9e6d81";
            string authToken = "eb08cbc74022c3139881c5ab975708e9";
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                   body:Message,
                   messagingServiceSid: "MGf93d3a9be3f958341abc532e51a1a9b0",
                   to: new Twilio.Types.PhoneNumber("whatsapp:" + To));
        }
    }
}
