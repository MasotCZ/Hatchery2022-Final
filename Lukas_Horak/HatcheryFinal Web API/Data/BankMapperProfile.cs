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
        }
    }
}
