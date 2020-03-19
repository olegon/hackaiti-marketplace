using System.Threading.Tasks;
using Checkout.Service.Worker.Models;
using Refit;

namespace Checkout.Service.Worker.Services
{
    public interface IInvoiceService
    {
        [Post("/invoices")]
        Task SendInvoice([Body]CartInvoiceRequest payload, [Header("x-team-control")]string id);
    }
}