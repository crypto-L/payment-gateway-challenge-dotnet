namespace PaymentGateway.Api.Common.Validation;

public interface IValidationRule<TEntity>
{
    ValidationFailure? Validate(TEntity entity);
}