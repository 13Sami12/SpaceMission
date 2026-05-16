namespace SpaceMission.Core
{
    using System;
    using System.Net;
    using System.Net.Mail;

    public record EmailSettings(
        string Host,
        int Port,
        string Sender,
        string Recipient,
        string Username,
        string Password,
        bool EnableSsl = true);

    public static class EmailService
    {
        public static MailMessage CreateMessage(EmailSettings settings, string body)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return new MailMessage(settings.Sender, settings.Recipient)
            {
                Subject = "SpaceMission summary",
                Body = body,
                IsBodyHtml = false
            };
        }

        public static void SendMissionSummary(EmailSettings settings, string body)
        {
            using var message = CreateMessage(settings, body);
            using var client = new SmtpClient(settings.Host, settings.Port)
            {
                EnableSsl = settings.EnableSsl,
                Credentials = new NetworkCredential(settings.Username, settings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            client.Send(message);
        }
    }
}
