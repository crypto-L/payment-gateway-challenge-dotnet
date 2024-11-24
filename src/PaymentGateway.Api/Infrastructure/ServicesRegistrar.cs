using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Infrastructure;

public static class ServicesRegistrar
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBankService, MountebankService>();
    }
}