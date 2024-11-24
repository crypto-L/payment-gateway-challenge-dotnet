using PaymentGateway.Api.Enums;

namespace PaymentGateway.Api.Domain;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public PaymentStatus Status { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string CurrencyCode { get; set; }
    public int Amount { get; set; }
    public int Cvv { get; set; }
}