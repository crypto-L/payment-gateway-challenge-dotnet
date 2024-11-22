using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public interface IBankIntegrationClient
{
    Task<PaymentAuthorizationResponse> ProcessPaymentAsync(PaymentRequest paymentRequest);

    public void RetrievePayment();
}