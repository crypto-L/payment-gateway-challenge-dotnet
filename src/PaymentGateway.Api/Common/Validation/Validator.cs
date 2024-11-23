namespace PaymentGateway.Api.Common.Validation;

public class Validator<TValidatedEntity>
{
    private readonly List<IValidationRule<TValidatedEntity>> _rules = [];
    private readonly List<ValidationFailure> _errors = [];
    
    public void AddRule(IValidationRule<TValidatedEntity> rule)
    {
        _rules.Add(rule);
    }

    public List<ValidationFailure> Validate(TValidatedEntity entity)
    {
        _errors.Clear();
        
        foreach (var rule in _rules)
        {
            var failure = rule.Validate(entity);
            if (failure != null) _errors.Add(failure);
        }

        return _errors;
    }
}