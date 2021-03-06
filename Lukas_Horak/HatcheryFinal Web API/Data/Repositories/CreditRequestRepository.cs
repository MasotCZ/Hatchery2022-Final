using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data
{
    class CreditRequestRepository : RepositoryBase<CreditRequest, CreditRequestRepository>, ICreditRequestRepository
    {
        public CreditRequestRepository(IBankDbContext context, ILogger<CreditRequestRepository> logger) : base(context, logger)
        {
        }

        public async Task<CreditRequest[]> GetAllUnfulfilledActiveCreditRequestsAsync()
        {
            _logger.LogInformation($"Selecting unfulfilled credit requests");

            var querry = _context.CreditRequests.AsQueryable();

            querry = querry
                .Where(d => d.ContactStatus == null || d.ContactStatus.StatusCode == CreditRequestStatusCode.Unfulfilled)
                .Include(d => d.ContactStatus)
                .Include(d => d.Partner)
                .Where(d => d.Partner.StartDate < DateTime.Now && (d.Partner.EndDate == null || d.Partner.EndDate > DateTime.Now));

            var res = await querry.ToArrayAsync();

            _logger.LogInformation("Found unfulfilled credit requests: {0}", res is null ? 0 : res.Length);

            return res;
        }

        public async Task<CreditRequest> GetCreditRequestByIdAsync(int id)
        {
            _logger.LogInformation($"Selecting credit request by id: {id}");

            var querry = _context.CreditRequests.AsQueryable();

            querry = querry
                .Where(d => d.Id == id)
                .Include(d => d.ContactStatus);

            var res = await querry.FirstOrDefaultAsync();

            _logger.LogInformation($"Found credit request:{res is not null}");

            return res;
        }
    }
}
