using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data
{
    class CreditRequestRepository : RepositoryBase<CreditRequest>, ICreditRequestRepository
    {
        public CreditRequestRepository(BankDbContext context, ILogger<CreditRequestRepository> logger) : base(context, logger)
        {
        }

        public async Task<CreditRequest[]> GetAllUnfulfilledCreditRequestsAsync()
        {
            _logger.LogInformation($"Selecting unfulfilled credit requests");

            var querry = _context.CreditRequests.AsQueryable();

            querry = querry.Where(d => d.ContactStatus.StatusCode == CreditRequestStatusCode.Unfulfilled);

            var res = await querry.ToArrayAsync();

            _logger.LogInformation($"Found unfulfilled credit requests: {res.Length}");

            return res;
        }
    }
}
