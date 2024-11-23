using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services;

public interface IBankService
{
    Task ProcessPayment(PostPaymentRequest paymentRequest);

}