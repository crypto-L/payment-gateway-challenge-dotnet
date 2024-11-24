using System.Text.Json;

using AutoMapper;

using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.Integrations;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services.Validation;

namespace PaymentGateway.Api.Services;

public class MountebankService : IBankService
{
    private readonly MountebankClient _mountebankClient;
    private readonly IMapper _mapper;

    public MountebankService(MountebankClient mountebankClient, IMapper mapper)
    {
        _mountebankClient = mountebankClient;
        _mapper = mapper;
    }

    public async Task ProcessPayment(PostPaymentRequest paymentRequest)
    {
        var validationErrors = ValidatePaymentRequest(paymentRequest);

        if (validationErrors.Count > 0)
        {
            // return not processed
        }

        var externalPaymentRequest = _mapper.Map<ExternalPaymentRequest>(paymentRequest);
        
        var paymentResult = await _mountebankClient.ProcessPaymentAsync(externalPaymentRequest);
        
        
        Console.WriteLine($"Result: {JsonSerializer.Serialize(paymentResult)}");
    }

    private List<ValidationFailure> ValidatePaymentRequest(PostPaymentRequest paymentRequest)
    {
        var validator = new Validator<PostPaymentRequest>();
        
        validator.AddRule(new CardExpiryDateValidationRule());
        validator.AddRule(new CurrencyCodeValidationRule());

        var errors = validator.Validate(paymentRequest);

        Console.WriteLine($"Errors: {errors.Count}");
        
        return errors;
    }
}