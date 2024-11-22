using System.Text.Json;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Integrations;

public static class ApiResponseHelper
{
    public static async Task<PaymentAuthorizationResponse> HandleApiResponse(HttpResponseMessage response, JsonSerializerOptions serializerOptions, bool isErrorExpected = false)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        var result = new PaymentAuthorizationResponse
        {
            StatusCode = response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            result.Details = JsonSerializer.Deserialize<PaymentAuthorizationResponse.AuthorizationDetails>(content, serializerOptions);
        }
        else
        {
            result.ErrorMessage = content;
        }

        return result;
    }
}