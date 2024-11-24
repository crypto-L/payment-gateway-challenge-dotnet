using PaymentGateway.Api.DAL;

namespace PaymentGateway.Api.Infrastructure;

public static class RepoRegistrar
{
    public static void RegisterRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<PaymentsRepository>();
    }
}