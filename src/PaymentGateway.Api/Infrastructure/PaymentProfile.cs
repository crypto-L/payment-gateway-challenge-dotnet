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
        
        CreateMap<PostPaymentRequest, PostPaymentResponse>()
            .ForMember(dest => dest.CardNumberLastFour, opt => opt.MapFrom(src => GetLastFourDigits(src.CardNumber)))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => src.ExpiryMonth))
            .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => src.ExpiryYear))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<(ExternalPaymentRequest Request, ExternalPaymentAuthorizationResponse Response), Payment>()
            .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Request.CardNumber))
            .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => int.Parse(src.Request.ExpiryDate.Substring(0, 2))))
            .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => int.Parse(src.Request.ExpiryDate.Substring(3, 4))))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Request.Currency))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Request.Amount))
            .ForMember(dest => dest.Cvv, opt => opt.MapFrom(src => src.Request.Cvv))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetPaymentStatus(src.Response)))
            .ForMember(dest => dest.AuthorizationCode, opt => opt.MapFrom(src => src.Response.Details != null ? src.Response.Details.AuthorizationCode : null))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<Payment, GetPaymentResponse>()
            .ForMember(dest => dest.CardNumberLastFour, opt => opt.MapFrom(src => GetLastFourDigits(src.CardNumber)))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.CurrencyCode))
            .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => src.ExpiryMonth))
            .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => src.ExpiryYear))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
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
    
    private int GetLastFourDigits(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
        {
            throw new ArgumentException("Card number is null or empty.");
        }
        
        if (cardNumber.Length < 4)
        {
            throw new ArgumentException("Card number must have at least 4 digits.");
        }
        
        if (!cardNumber.All(char.IsDigit))
        {
            throw new ArgumentException("Card number contains invalid characters. It must contain only numeric digits.");
        }
        
        return int.Parse(cardNumber.Substring(cardNumber.Length - 4));
    }
}