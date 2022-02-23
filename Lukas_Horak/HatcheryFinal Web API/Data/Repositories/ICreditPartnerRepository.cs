using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    /// <summary>
    /// Interface for credit partners on DbContext.
    /// </summary>
    public interface ICreditPartnerRepository : IRepository<CreditPartner>
    {
        /// <summary>
        /// </summary>
        /// <param name="token">Unique token of the partner</param>
        /// <param name="includeRequests">whether to include requests with partner or not</param>
        /// <returns>Partner associated with the token if he is registered or active, otherwise <see cref="null"/></returns>
        public Task<CreditPartner> GetActiveCreditPartnerByTokenAsync(string token, bool includeRequests = false);

        /// <summary>
        /// </summary>
        /// <param name="token">Unique token of the partner</param>
        /// <param name="includeRequests">whether to include requests with partner or not</param>
        /// <returns>Partner associated with the token if he is active or not, otherwise <see cref="null"/> if not registered</returns>
        public Task<CreditPartner> GetCreditPartnerByTokenAsync(string token, bool includeRequests = false);

        /// <summary>
        /// </summary>
        /// <param name="token">Unique id of the partner</param>
        /// <param name="includeRequests">whether to include requests with partner or not</param>
        /// <returns>Partner associated with the id if he is active or not, otherwise <see cref="null"/> if not registered</returns>
        public Task<CreditPartner> GetCreditPartnerByIdAsync(int id, bool includeRequests = false);
    }
}
