using AutoMapper;

using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.DAL;
using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Integrations;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services.Validation;

namespace PaymentGateway.Api.Services;

public class MountebankService : IBankService
{
    private readonly MountebankClient _mountebankClient;
    private readonly PaymentsRepository _paymentsRepository;
    private readonly IMapper _mapper;

    public MountebankService(MountebankClient mountebankClient, PaymentsRepository paymentsRepository,IMapper mapper)
    {
        _mountebankClient = mountebankClient;
        _paymentsRepository = paymentsRepository;
        _mapper = mapper;
    }

    public async Task<Guid?> ProcessPayment(PostPaymentRequest paymentRequest)
    {
        var validationErrors = ValidatePaymentRequest(paymentRequest);

        if (validationErrors.Count > 0)
        {
            return null;
        }

        var externalPaymentRequest = _mapper.Map<ExternalPaymentRequest>(paymentRequest);
        
        var externalPaymentResponse = await _mountebankClient.ProcessPaymentAsync(externalPaymentRequest);
        
        var payment = _mapper.Map<(ExternalPaymentRequest, ExternalPaymentAuthorizationResponse), Payment>((externalPaymentRequest, externalPaymentResponse));
        
        _paymentsRepository.Add(payment);

        return payment.Id;
    }

    public GetPaymentResponse? RetrievePayment(Guid id)
    {
        var payment = _paymentsRepository.Get(id);
        if (payment == null)
        {
            return null;
        }
        var getPaymentResponse = _mapper.Map<GetPaymentResponse>(payment);
        
        return getPaymentResponse;
    }

    public PostPaymentResponse CreateRejectedPostResponse(PostPaymentRequest request)
    {
        var response = _mapper.Map<PostPaymentResponse>(request);
        response.Status = PaymentStatus.Rejected;
        return response;
    }

    private List<ValidationFailure> ValidatePaymentRequest(PostPaymentRequest paymentRequest)
    {
        var validator = new Validator<PostPaymentRequest>();
        
        validator.AddRule(new CardExpiryDateValidationRule());
        validator.AddRule(new CurrencyCodeValidationRule());

        var errors = validator.Validate(paymentRequest);
        
        return errors;
    }
}