using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public interface IBankIntegrationClient
{
    public BankTransactionResult ProcessPayment();

    public void RetrievePayment();
}