using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    public interface ICreditPartnerRepository : IRepository<CreditPartner>
    {
        public Task<CreditPartner> GetCreditPartnerByTokenAsync(Guid token);
        public Task<CreditPartner> GetActiveCreditPartnerByTokenAsync(Guid token);
        public Task<CreditPartner> GetInactiveCreditPartnerByTokenASync(Guid token);
        public Task<CreditPartner> GetCreditPartnerByIdAsync(int id);
    }
}
