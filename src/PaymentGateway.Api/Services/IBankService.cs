using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services;

public interface IBankService
{
    Task<Guid?> ProcessPayment(PostPaymentRequest paymentRequest);

    T? RetrievePayment<T>(Guid id) where T : class;

}