using System.Net;

using AutoMapper;

using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Infrastructure;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<PostPaymentRequest, ExternalPaymentRequest>()
            .ForMember(
                dest => dest.ExpiryDate,
                opt => opt.MapFrom(
                    src => $"{src.ExpiryMonth:D2}/{src.ExpiryYear}"));
        
        
        CreateMap<(ExternalPaymentRequest Request, ExternalPaymentAuthorizationResponse Response), Payment>()
            .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Request.CardNumber))
            .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => int.Parse(src.Request.ExpiryDate.Substring(0, 2))))
            .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => int.Parse(src.Request.ExpiryDate.Substring(3, 4))))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Request.Currency))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Request.Amount))
            .ForMember(dest => dest.Cvv, opt => opt.MapFrom(src => src.Request.Cvv))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetPaymentStatus(src.Response)))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        

    }
    
    private PaymentStatus GetPaymentStatus(ExternalPaymentAuthorizationResponse response)
    {
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return response.Details?.Authorized == true 
                ? PaymentStatus.Authorized 
                : PaymentStatus.Declined;
        }

        return PaymentStatus.Rejected;
    }
}