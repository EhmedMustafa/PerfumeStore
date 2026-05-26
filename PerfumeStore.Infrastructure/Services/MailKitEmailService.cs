using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using PerfumeStore.Application.Interfaces;

namespace PerfumeStore.Infrastructure.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MailKitEmailService> _logger;

        public MailKitEmailService(IConfiguration config, ILogger<MailKitEmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendAsync(string to, string subject, string htmlBody, string textBody = null)
        {
            if (string.IsNullOrWhiteSpace(to)) return false;

            var section = _config.GetSection("Email");
            var host = section["SmtpHost"];
            var portStr = section["SmtpPort"];
            var user = section["Username"];
            var pass = section["Password"];
            var fromEmail = section["FromEmail"];
            var fromName = section["FromName"] ?? "OMAR PERFUME";

            // SMTP konfiqurasiyası boşdursa, email göndərmə (səssiz keç)
            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(fromEmail))
            {
                _logger.LogWarning("Email config not set — skipping send to {to}", to);
                return false;
            }

            var port = int.TryParse(portStr, out var p) ? p : 587;

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = textBody ?? StripHtml(htmlBody)
                };
                message.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                var secureOption = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                await smtp.ConnectAsync(host, port, secureOption);
                if (!string.IsNullOrWhiteSpace(user))
                    await smtp.AuthenticateAsync(user, pass);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Email sent to {to} — {subject}", to, subject);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email send failed to {to}", to);
                return false;
            }
        }

        public Task<bool> SendOrderConfirmationAsync(string to, int orderId, string customerName, decimal total)
        {
            var subject = $"Sifarişiniz qəbul edildi — #{orderId}";
            var html = $@"
<div style='font-family:Outfit,Arial,sans-serif;background:#faf8f5;padding:40px 20px;color:#2c2420;'>
  <div style='max-width:560px;margin:0 auto;background:#fff;border-radius:12px;padding:40px 36px;border:1px solid #e0d8cc;'>
    <div style='text-align:center;margin-bottom:32px;'>
      <h1 style='font-family:Cormorant Garamond,Georgia,serif;font-size:2rem;letter-spacing:0.5em;margin:0;color:#8b6914;'>OMAR</h1>
      <div style='font-size:0.75rem;letter-spacing:0.3em;color:#8b6914;margin-top:4px;'>PERFUME</div>
    </div>
    <h2 style='font-family:Cormorant Garamond,Georgia,serif;font-weight:400;color:#2c2420;'>Salam, {Escape(customerName)}!</h2>
    <p style='line-height:1.7;color:#5a4e42;'>Sifarişiniz uğurla qeydə alındı və hazırlığa başlanıb. Ən qısa zamanda sizinlə əlaqə saxlayacağıq.</p>
    <div style='background:#faf8f5;border:1px solid #e0d8cc;border-radius:8px;padding:20px;margin:24px 0;'>
      <div style='display:flex;justify-content:space-between;margin-bottom:8px;'>
        <span style='color:#7a6e62;'>Sifariş nömrəsi:</span>
        <strong>#{orderId}</strong>
      </div>
      <div style='display:flex;justify-content:space-between;'>
        <span style='color:#7a6e62;'>Ümumi məbləğ:</span>
        <strong style='color:#8b6914;'>₼{total:0.00}</strong>
      </div>
    </div>
    <p style='line-height:1.7;color:#5a4e42;font-size:0.9rem;'>
      Sifarişinizin statusunu hesabınızdan izləyə bilərsiniz. Hər hansı sual varsa, WhatsApp ilə əlaqə saxlayın:
      <a href='https://wa.me/994502330644' style='color:#8b6914;'>+994 50 233 06 44</a>
    </p>
    <div style='border-top:1px solid #e0d8cc;margin-top:32px;padding-top:20px;text-align:center;color:#a89c8e;font-size:0.78rem;letter-spacing:0.2em;'>
      OMAR PERFUME — Premium parfüm
    </div>
  </div>
</div>";
            return SendAsync(to, subject, html);
        }

        public Task<bool> SendOrderStatusAsync(string to, int orderId, string customerName, string statusText)
        {
            var subject = $"Sifariş statusu yeniləndi — #{orderId}";
            var html = $@"
<div style='font-family:Outfit,Arial,sans-serif;background:#faf8f5;padding:40px 20px;color:#2c2420;'>
  <div style='max-width:560px;margin:0 auto;background:#fff;border-radius:12px;padding:40px 36px;border:1px solid #e0d8cc;'>
    <div style='text-align:center;margin-bottom:32px;'>
      <h1 style='font-family:Cormorant Garamond,Georgia,serif;font-size:2rem;letter-spacing:0.5em;margin:0;color:#8b6914;'>OMAR</h1>
      <div style='font-size:0.75rem;letter-spacing:0.3em;color:#8b6914;margin-top:4px;'>PERFUME</div>
    </div>
    <h2 style='font-family:Cormorant Garamond,Georgia,serif;font-weight:400;'>Salam, {Escape(customerName)}!</h2>
    <p style='line-height:1.7;color:#5a4e42;'>Sifariş <strong>#{orderId}</strong> statusu dəyişdirildi:</p>
    <div style='background:#fdf6e3;border:1px solid #c9a96e;border-radius:8px;padding:20px;margin:24px 0;text-align:center;'>
      <div style='font-size:0.78rem;letter-spacing:0.25em;color:#8b6914;text-transform:uppercase;margin-bottom:8px;'>Yeni Status</div>
      <div style='font-family:Cormorant Garamond,Georgia,serif;font-size:1.6rem;color:#6d510e;'>{Escape(statusText)}</div>
    </div>
    <p style='line-height:1.7;color:#5a4e42;font-size:0.9rem;'>
      Sual varsa: <a href='https://wa.me/994502330644' style='color:#8b6914;'>+994 50 233 06 44</a>
    </p>
    <div style='border-top:1px solid #e0d8cc;margin-top:32px;padding-top:20px;text-align:center;color:#a89c8e;font-size:0.78rem;letter-spacing:0.2em;'>
      OMAR PERFUME
    </div>
  </div>
</div>";
            return SendAsync(to, subject, html);
        }

        private static string Escape(string s) => string.IsNullOrEmpty(s) ? "" : s
            .Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

        private static string StripHtml(string html) => System.Text.RegularExpressions.Regex
            .Replace(html ?? "", "<[^>]+>", " ");
    }
}
