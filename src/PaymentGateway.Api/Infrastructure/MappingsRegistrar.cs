namespace PaymentGateway.Api.Infrastructure;

public static class MappingsRegistrar
{
    public static void RegisterMappings(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(PaymentProfile));
    }
}