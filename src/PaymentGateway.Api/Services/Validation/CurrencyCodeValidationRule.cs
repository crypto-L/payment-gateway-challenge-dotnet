using ISO._4217;

using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Services.Validation;

public class CurrencyCodeValidationRule : IValidationRule<PostPaymentRequest>
{
    public ValidationFailure? Validate(PostPaymentRequest entity)
    {
        var isoCodeExists = CurrencyCodesResolver.GetCurrenciesByCode(entity.Currency).ToList().Any();
        
        return isoCodeExists ? null : new ValidationFailure("Invalid currency ISO code.");
    }
}