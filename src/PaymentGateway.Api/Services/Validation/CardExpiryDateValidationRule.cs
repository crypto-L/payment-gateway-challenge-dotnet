using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Services.Validation;

public class CardExpiryDateValidationRule : IValidationRule<PostPaymentRequest>
{
    public ValidationFailure? Validate(PostPaymentRequest entity)
    {
        if (entity.ExpiryMonth is < 1 or > 12)
        {
            return new ValidationFailure("Expiry month must be between 1 and 12.");
        }

        var expiryDate = new DateTime(entity.ExpiryYear, entity.ExpiryMonth, 1, 0, 0, 0, DateTimeKind.Utc);
        
        return expiryDate <= DateTime.UtcNow ? new ValidationFailure("Expiry date must be in the future.") : null;
    }
}