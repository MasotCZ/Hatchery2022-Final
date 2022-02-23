using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    /// <summary>
    /// Interface for credit requests on DbContext.
    /// </summary>
    public interface ICreditRequestRepository : IRepository<CreditRequest>
    {
        /// <summary>
        /// </summary>
        /// <returns> All unfulfilled requests that are in the system, unless the partner is inactive </returns>
        public Task<CreditRequest[]> GetAllUnfulfilledActiveCreditRequestsAsync();

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Credit request by id not matter its status or partners status</returns>
        public Task<CreditRequest> GetCreditRequestByIdAsync(int id);
    }
}
