using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Common.Validation;
using PaymentGateway.Api.Controllers.Validation;
using PaymentGateway.Api.DAL;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly IBankService _mountebankService;

    public PaymentsController(IBankService mountebankService)
    {
        _mountebankService = mountebankService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _mountebankService.RetrievePayment(id);
        
        if (payment == null)
        {
            return NotFound(new { Message = "Payment not found." });
        }
        return new OkObjectResult(payment);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> PostPaymentAsync(PostPaymentRequest paymentRequest)
    {
        var validator = new Validator<PostPaymentRequest>();
        validator.AddRule(new PostPaymentRequestValidationRule());
        
        var errors = validator.Validate(paymentRequest);
        if (errors.Count > 0)
        {
            return BadRequest(_mountebankService.CreateRejectedPostResponse(paymentRequest));
        }
        
        var paymentId = await _mountebankService.ProcessPayment(paymentRequest);
        if (!paymentId.HasValue)
        {
            return BadRequest(_mountebankService.CreateRejectedPostResponse(paymentRequest));
        }

        var payment = _mountebankService.RetrievePayment(paymentId.Value);

        return new OkObjectResult(payment);
    }
}