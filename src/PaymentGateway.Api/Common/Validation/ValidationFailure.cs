namespace PaymentGateway.Api.Common.Validation;

public class ValidationFailure
{
    public string Description { get; set; }

    public ValidationFailure(string description)
    {
        Description = description;
    }
}