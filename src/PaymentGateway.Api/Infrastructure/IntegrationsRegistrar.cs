using PaymentGateway.Api.Integrations;

namespace PaymentGateway.Api.Infrastructure;

public static class IntegrationsRegistrar
{
    public static void RegisterExternalServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBankIntegrationClient, MountebankClient>();
    }
}