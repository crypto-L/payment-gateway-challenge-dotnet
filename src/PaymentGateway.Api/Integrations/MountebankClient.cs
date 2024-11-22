using System.Text.Json;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public class MountebankClient : IBankIntegrationClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    

    public MountebankClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PaymentAuthorizationResponse> ProcessPaymentAsync(PaymentRequest paymentRequest)
    {
        var client = _httpClientFactory.CreateClient();

        var serializedRequest = JsonSerializer.Serialize(paymentRequest, _serializerOptions);
        var url = $"{_configuration["MountebankIntegration:BaseUrl"]}/payments";

        var content = new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(url, content);
        
        var e = await ApiResponseHelper.HandleApiResponse(response, _serializerOptions);
        Console.WriteLine(e.StatusCode);
        Console.WriteLine(e.ErrorMessage);
        
        
        return new PaymentAuthorizationResponse();
    }

    public void RetrievePayment()
    {
        throw new NotImplementedException();
    }
}