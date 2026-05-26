using System.Threading.Tasks;

namespace PerfumeStore.Application.Interfaces
{
    public interface IEmailService
    {
        // Universal göndərmə metodu
        Task<bool> SendAsync(string to, string subject, string htmlBody, string textBody = null);

        // Hazır şablonlar
        Task<bool> SendOrderConfirmationAsync(string to, int orderId, string customerName, decimal total);
        Task<bool> SendOrderStatusAsync(string to, int orderId, string customerName, string statusText);
    }
}
