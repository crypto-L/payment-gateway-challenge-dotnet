using System.Text.Json;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public static class ApiResponseHelper
{
    public static async Task<ExternalPaymentAuthorizationResponse> HandleApiResponse(HttpResponseMessage response, JsonSerializerOptions serializerOptions)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        var result = new ExternalPaymentAuthorizationResponse
        {
            StatusCode = response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            result.Details = JsonSerializer.Deserialize<ExternalPaymentAuthorizationResponse.AuthorizationDetails>(content, serializerOptions);
        }
        else
        {
            result.ErrorMessage = content;
        }

        return result;
    }
}