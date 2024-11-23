using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Requests;

public class ExternalPaymentRequest
{
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public string Cvv { get; set; }
}