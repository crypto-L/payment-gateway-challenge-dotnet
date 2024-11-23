using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services;

public class PaymentsRepository
{
    public List<PostPaymentResponse> Payments = new();
    
    public PaymentsRepository()
    {
        Add(new PostPaymentResponse
        {
            Id = Guid.Parse("5f93664c-863a-4665-a67b-6e83ccaf82f6"),
            Status = PaymentStatus.Authorized,
            CardNumberLastFour = 1234,
            ExpiryMonth = 12,
            ExpiryYear = 2025,
            Currency = "USD",
            Amount = 1000
        });

        Add(new PostPaymentResponse
        {
            Id = Guid.Parse("5b0c281f-1aff-41cb-86b6-46b0e3a53e49"),
            Status = PaymentStatus.Declined,
            CardNumberLastFour = 5678,
            ExpiryMonth = 6,
            ExpiryYear = 2024,
            Currency = "EUR",
            Amount = 500
        });
    }
    
    public void Add(PostPaymentResponse payment)
    {
        Payments.Add(payment);
    }

    public PostPaymentResponse Get(Guid id)
    {
        return Payments.FirstOrDefault(p => p.Id == id);
    }
}