using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    /// <summary>
    /// Interface for credit reports on DbContext.
    /// </summary>
    public interface IProfitabilityRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="includeRequests">whether to include requests with partner or not</param>
        /// <returns>Most profitable partner in DbContext.
        /// If no partner with profits exists a random one is returned.
        /// If no partner with requests exists a null is returned </returns>
        /// </returns>
        public Task<CreditPartner> GetMostProfitablePartnerAsync(bool includeRequests = false);

        /// <summary>
        /// </summary>
        /// <param name="includeRequests">whether to include requests with partner or not</param>
        /// <returns>
        /// Most successful partner in DbContext.
        /// If no partner with profits exists a random one is returned.
        /// If no partner with requests exists a null is returned.
        /// </returns>
        public Task<CreditPartner> GetMostSuccessfulPartnerAsync(bool includeRequests = false);
    }
}
