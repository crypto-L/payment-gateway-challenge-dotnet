using System.Net;

namespace PaymentGateway.Api.Models.Responses;

public class ExternalPaymentAuthorizationResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public AuthorizationDetails? Details { get; set; }
    public string? ErrorMessage { get; set; }
    public class AuthorizationDetails
    {
        public bool Authorized { get; set; }
        public string? AuthorizationCode { get; set; }
    }
   
}
