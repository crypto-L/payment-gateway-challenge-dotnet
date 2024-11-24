using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.DAL;
using PaymentGateway.Api.Integrations;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly PaymentsRepository _paymentsRepository;
    private readonly MountebankService _mountebankService;

    public PaymentsController(PaymentsRepository paymentsRepository, MountebankService mountebankService)
    {
        _paymentsRepository = paymentsRepository;
        _mountebankService = mountebankService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _paymentsRepository.Get(id);

        return new OkObjectResult(payment);
    }

    [HttpGet("test")]
    public async Task<ActionResult> Test()
    {
        var authReq = new PostPaymentRequest()
        {
            CardNumber = "2222405343248877",
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            Currency = "GBP",
            Amount = 100,
            Cvv = 123
        };
        
        var notAuthReq = new PostPaymentRequest()
        {
            CardNumber = "2222405343248112",
            ExpiryMonth = 1,
            ExpiryYear = 2026,
            Currency = "USD",
            Amount = 60000,
            Cvv = 456
        };
        
        var badReq = new PostPaymentRequest()
        {
            CardNumber = "2222405343248877",
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            Currency = "GBP",
            Amount = 101,
            Cvv = 123
        };
        
        var paymentId = await _mountebankService.ProcessPayment(notAuthReq);
        if (paymentId.HasValue)
        {
            var payment = _paymentsRepository.Get(paymentId.Value);
            Console.WriteLine(JsonSerializer.Serialize(payment));
        }
        return Ok(paymentId);
    }
}