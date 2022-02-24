using HatcheryFinal_Web_API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data.Repositories
{
    public class ProfitabilityRepository : IProfitabilityRepository
    {
        protected readonly IBankDbContext _context;
        protected readonly ILogger<ProfitabilityRepository> _logger;

        public ProfitabilityRepository(IBankDbContext context, ILogger<ProfitabilityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<CreditPartner> GetMostProfitablePartnerAsync(bool includeRequests = false)
        {
            _logger.LogInformation($"Selecting most profitable credit partner");

            var res = _context.CreditPartners
               .AsQueryable();

            if (includeRequests)
            {
                res = res
                    .Include(d => d.Requests)
                    .ThenInclude(d => d.ContactStatus);
            }

            res = res.Where(d => d.Requests != null && d.Requests.Count != 0)
            .OrderByDescending(d => d.Requests
                    .Where(r => r.ContactStatus.StatusCode == CreditRequestStatusCode.Accepted)
                    .Sum(r => r.Credit)
                );

            return res.FirstOrDefaultAsync();
        }

        public Task<CreditPartner> GetMostSuccessfulPartnerAsync(bool includeRequests = false)
        {
            _logger.LogInformation($"Selecting most successful credit partner");

            var res = _context.CreditPartners
                .AsQueryable();

            if (includeRequests)
            {
                res = res
                    .Include(d => d.Requests)
                    .ThenInclude(d => d.ContactStatus);
            }

            res = res.Where(d => d.Requests != null && d.Requests.Count != 0)
            .OrderByDescending(d => d.Requests
                    .Where(r => r.ContactStatus.StatusCode == CreditRequestStatusCode.Accepted)
                    .Count() / d.Requests.Count
                );

            return res.FirstOrDefaultAsync();
        }
    }
}
