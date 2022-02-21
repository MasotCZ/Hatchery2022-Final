using HatcheryFinal_Web_API.Data.Entities;

namespace HatcheryFinal_Web_API.Data.Repositories
{

    public interface ICreditRequestRepository : IRepository<CreditRequest>
    {
        public Task<CreditRequest[]> GetAllUnfulfilledCreditRequestsAsync(); 
    }
}
