using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Controllers.Validation;

public class PostPaymentRequestValidationRule : IValidationRule<PostPaymentRequest>
{
    public ValidationFailure? Validate(PostPaymentRequest entity)
    {
        // Card number
        if (string.IsNullOrEmpty(entity.CardNumber))
        {
            return new ValidationFailure("Card number is required.");
        }
        if (entity.CardNumber.Length is < 14 or > 19)
        {
            return new ValidationFailure("Card number must be between 14 and 19 characters long.");
        }
        if (!entity.CardNumber.All(char.IsDigit))
        {
            return new ValidationFailure("Card number must only contain numeric characters.");
        }

        // Expiry month
        if (entity.ExpiryMonth is < 1 or > 12)
        {
            return new ValidationFailure("Expiry month must be between 1 and 12.");
        }
        
        // Expiry year
        if (entity.ExpiryYear <= 0)
        {
            return new ValidationFailure("Expiry year is required.");
        }
        
        // Currency code
        if (string.IsNullOrEmpty(entity.Currency) || entity.Currency.Length != 3)
        {
            return new ValidationFailure("Currency must be 3 characters long.");
        }

        // Amount
        if (entity.Amount <= 0)
        {
            return new ValidationFailure("Amount must be greater than zero.");
        }
        
        // CVV
        if (entity.Cvv <= 0)
        {
            return new ValidationFailure("CVV is required and must be a positive integer.");
        }
        if (entity.Cvv.ToString().Length < 3 || entity.Cvv.ToString().Length > 4)
        {
            return new ValidationFailure("CVV must be 3 to 4 digits long.");
        }
        return null;
    }
}