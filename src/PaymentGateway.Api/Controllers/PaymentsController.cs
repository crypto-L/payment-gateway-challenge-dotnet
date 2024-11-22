using Microsoft.AspNetCore.Mvc;

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
    private readonly MountebankClient _mountebank;

    public PaymentsController(PaymentsRepository paymentsRepository, MountebankClient mountebankClient)
    {
        _paymentsRepository = paymentsRepository;
        _mountebank = mountebankClient;
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
        var authReq = new PaymentRequest()
        {
            CardNumber = "2222405343248877",
            ExpiryDate = "04/2025",
            Currency = "GBP",
            Amount = 100,
            Cvv = "123"
        };
        
        var notAuthReq = new PaymentRequest()
        {
            CardNumber = "2222405343248112",
            ExpiryDate = "01/2026",
            Currency = "USD",
            Amount = 60000,
            Cvv = "456"
        };
        
        var badReq = new PaymentRequest()
        {
            CardNumber = "2222405343248877",
            ExpiryDate = "04/2025",
            Currency = "GBP",
            Amount = 101,
            Cvv = "123"
        };
        await _mountebank.ProcessPaymentAsync(badReq);
        return Ok("Heh");
    }
}