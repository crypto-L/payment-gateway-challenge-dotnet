using PaymentGateway.Api.Infrastructure;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public class MountebankClient : IBankIntegrationClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public MountebankClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public BankTransactionResult ProcessPayment()
    {
        throw new NotImplementedException();
    }

    public void RetrievePayment()
    {
        throw new NotImplementedException();
    }
}