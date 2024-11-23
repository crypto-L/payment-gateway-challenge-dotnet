using AutoMapper;

using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Infrastructure;

public class PaymentRequestProfile : Profile
{
    public PaymentRequestProfile()
    {
        CreateMap<PostPaymentRequest, ExternalPaymentRequest>()
            .ForMember(
                dest => dest.ExpiryDate,
                opt => opt.MapFrom(
                    src => $"{src.ExpiryMonth:D2}/{src.ExpiryYear}"));
    }
}