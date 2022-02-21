using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    public interface ICreditPartnerRepository : IRepository<CreditPartner>
    {
        public Task<CreditPartner> GetCreditPartnerByTokenAsync(Guid token, bool includeRequests = false);
        public Task<CreditPartner> GetActiveCreditPartnerByTokenAsync(Guid token, bool includeRequests = false);
        public Task<CreditPartner> GetInactiveCreditPartnerByTokenASync(Guid token, bool includeRequests = false);
        public Task<CreditPartner> GetCreditPartnerByIdAsync(int id, bool includeRequests = false);
    }
}
