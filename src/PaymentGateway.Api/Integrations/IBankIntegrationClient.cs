using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public interface IBankIntegrationClient
{
    Task<ExternalPaymentAuthorizationResponse> ProcessPaymentAsync(ExternalPaymentRequest externalPaymentRequest);
}