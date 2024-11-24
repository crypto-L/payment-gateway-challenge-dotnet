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

    public async Task<ExternalPaymentAuthorizationResponse> ProcessPaymentAsync(ExternalPaymentRequest externalPaymentRequest)
    {
        var client = _httpClientFactory.CreateClient();

        var serializedRequest = JsonSerializer.Serialize(externalPaymentRequest, _serializerOptions);
        var url = $"{_configuration["MountebankIntegration:BaseUrl"]}/payments";

        var content = new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(url, content);
        
        return await ApiResponseHelper.HandleExternalApiResponse(response, _serializerOptions);
    }
}