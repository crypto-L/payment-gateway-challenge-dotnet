using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly Mock<IBankService> _mountebankServiceMock;
    
    public PaymentsControllerTests()
    {
        _mountebankServiceMock = new Mock<IBankService>();
    }

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var expectedPaymentResponse = new PaymentResponse
        {
            Id = paymentId,
            Amount = 1000,
            Currency = "USD",
            Status = "Authorized",
            ExpiryMonth = 12,
            ExpiryYear = 2025,
            CardNumberLastFour = 1234
        };
        
        _mountebankServiceMock.Setup(service => service.RetrievePayment(paymentId))
            .Returns(expectedPaymentResponse);
        
        var controller = new PaymentsController(_mountebankServiceMock.Object);
        
        // Act
        var result = await controller.GetPaymentAsync(paymentId);
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<PaymentResponse>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var actualPaymentResponse = Assert.IsType<PaymentResponse>(okResult.Value);
        
        Assert.Equal(expectedPaymentResponse.Id, actualPaymentResponse.Id);
        Assert.Equal(expectedPaymentResponse.Amount, actualPaymentResponse.Amount);
        Assert.Equal(expectedPaymentResponse.Currency, actualPaymentResponse.Currency);
        Assert.Equal(expectedPaymentResponse.Status, actualPaymentResponse.Status);
        Assert.Equal(expectedPaymentResponse.ExpiryMonth, actualPaymentResponse.ExpiryMonth);
        Assert.Equal(expectedPaymentResponse.ExpiryYear, actualPaymentResponse.ExpiryYear);
        Assert.Equal(expectedPaymentResponse.CardNumberLastFour, actualPaymentResponse.CardNumberLastFour);
    }
    
    [Fact]
    public async Task ReturnsNotFoundWhenPaymentDoesNotExist()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        _mountebankServiceMock.Setup(service => service.RetrievePayment(paymentId))
            .Returns((PaymentResponse?)null);

        var controller = new PaymentsController(_mountebankServiceMock.Object);

        // Act
        var result = await controller.GetPaymentAsync(paymentId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PaymentResponse>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }
    
    [Fact]
    public async Task ReturnsBadRequestWhenPaymentIdIsEmpty()
    {
        // Arrange
        var paymentId = Guid.Empty;
        var controller = new PaymentsController(_mountebankServiceMock.Object);

        // Act
        var result = await controller.GetPaymentAsync(paymentId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PaymentResponse>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }
    
    [Fact]
    public async Task ReturnsOkWhenPaymentIsProcessedSuccessfully()
    {
        // Arrange
        var paymentRequest = new PostPaymentRequest
        {
            CardNumber = "4111111111111111", 
            ExpiryMonth = 12,
            ExpiryYear = 2025,
            Currency = "USD",
            Amount = 1000,
            Cvv = 123
        };

        var paymentId = Guid.NewGuid();
        var expectedPaymentResponse = new PaymentResponse
        {
            Id = paymentId,
            Amount = 1000,
            Currency = "USD",
            Status = "Authorized",
            ExpiryMonth = 12,
            ExpiryYear = 2025,
            CardNumberLastFour = 1111
        };

        _mountebankServiceMock.Setup(service => service.ProcessPayment(paymentRequest))
            .ReturnsAsync(paymentId);

        _mountebankServiceMock.Setup(service => service.RetrievePayment(paymentId))
            .Returns(expectedPaymentResponse);

        var controller = new PaymentsController(_mountebankServiceMock.Object);

        // Act
        var result = await controller.PostPaymentAsync(paymentRequest);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PaymentResponse>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var paymentResponse = Assert.IsType<PaymentResponse>(okResult.Value);

        Assert.Equal(expectedPaymentResponse.Id, paymentResponse.Id);
        Assert.Equal(expectedPaymentResponse.Amount, paymentResponse.Amount);
        Assert.Equal(expectedPaymentResponse.Currency, paymentResponse.Currency);
        Assert.Equal(expectedPaymentResponse.Status, paymentResponse.Status);
    }

    
    [Fact]
    public async Task ReturnsBadRequestWhenPaymentProcessingFails()
    {
        // Arrange
        var paymentRequest = new PostPaymentRequest
        {
            CardNumber = "4111111111111111", 
            ExpiryMonth = 12,
            ExpiryYear = 2025,
            Currency = "USD",
            Amount = 1000,
            Cvv = 123
        };
        
        _mountebankServiceMock.Setup(service => service.ProcessPayment(paymentRequest))
            .ReturnsAsync((Guid?)null);

        _mountebankServiceMock.Setup(service => service.CreateRejectedPostResponse(paymentRequest))
            .Returns(new PaymentResponse 
            { 
                Id = Guid.NewGuid(), 
                Amount = 1000, 
                Currency = "USD", 
                Status = "Rejected", 
                ExpiryMonth = 12, 
                ExpiryYear = 2025, 
                CardNumberLastFour = 1111
            });

        var controller = new PaymentsController(_mountebankServiceMock.Object);

        // Act
        var result = await controller.PostPaymentAsync(paymentRequest);

        // Assert
        var actionResult = Assert.IsType<ActionResult<PaymentResponse>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        var rejectedResponse = Assert.IsType<PaymentResponse>(badRequestResult.Value);
        
        Assert.Equal("Rejected", rejectedResponse.Status);
        Assert.Equal(1000, rejectedResponse.Amount);
        Assert.Equal("USD", rejectedResponse.Currency);
    }
}