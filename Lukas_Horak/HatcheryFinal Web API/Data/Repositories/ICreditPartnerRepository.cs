using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    public interface ICreditPartnerRepository : IRepository<CreditPartner>
    {
        public Task<CreditPartner> GetActiveCreditPartnerByTokenAsync(string token, bool includeRequests = false);
        public Task<CreditPartner> GetCreditPartnerByTokenAsync(string token, bool includeRequests = false);
        public Task<CreditPartner> GetCreditPartnerByIdAsync(int id, bool includeRequests = false);
    }
}
