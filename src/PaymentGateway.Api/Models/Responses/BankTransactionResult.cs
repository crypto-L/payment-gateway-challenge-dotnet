namespace PaymentGateway.Api.Models.Responses;

public class BankTransactionResult
{
    public bool Authorized { get; set; }
    public Guid? AuthorizationCode { get; set; }
}