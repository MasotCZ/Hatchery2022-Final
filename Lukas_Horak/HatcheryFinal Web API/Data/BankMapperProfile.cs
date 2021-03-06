using AutoMapper;
using HatcheryFinal_Web_API.Data.Dto;
using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data
{
    public class BankMapperProfile : Profile
    {
        public BankMapperProfile()
        {
            this.CreateMap<CreditPartner, CreditPartnerFullInfoDto>()
                .ReverseMap();

            this.CreateMap<CreditRequest, CreditRequestDto>()
                .ReverseMap();

            this.CreateMap<CreditRequestStatus, CreditRequestStatusDto>()
                .ReverseMap();

            this.CreateMap<CreditPartner, CreditPartnerRegisteredDto>();

            this.CreateMap<CreditRequestStatusChangeIncomingDto, CreditRequest>()
                .ReverseMap();

            this.CreateMap<CreditPartnerChangeEndDateIncomingDto, CreditPartner>();

            this.CreateMap<CreditRequest, CreditRequestOutgoingWithIdDto>();

            this.CreateMap<CreditRequestNewIncomingDto, CreditRequest>();
        }
    }
}
