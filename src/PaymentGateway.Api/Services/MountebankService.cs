using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services.Validation;

namespace PaymentGateway.Api.Services;

public class MountebankService : IBankService
{
    public Task ProcessPayment(PostPaymentRequest paymentRequest)
    {
        var validationErrors = ValidatePaymentRequest(paymentRequest);

        return Task.CompletedTask;
    }

    private List<ValidationFailure> ValidatePaymentRequest(PostPaymentRequest paymentRequest)
    {
        var validator = new Validator<PostPaymentRequest>();
        
        validator.AddRule(new CardExpiryDateValidationRule());

        var errors = validator.Validate(paymentRequest);

        Console.WriteLine($"Errors: {errors.Count}");
        
        return errors;
    }
}