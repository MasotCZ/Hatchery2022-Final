using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data
{
    class CreditPartnerRepository : RepositoryBase<CreditPartner, CreditPartnerRepository>, ICreditPartnerRepository
    {
        public CreditPartnerRepository(BankDbContext context, ILogger<CreditPartnerRepository> logger) : base(context, logger)
        {
        }

        public async Task<CreditPartner> GetCreditPartnerByIdAsync(int id, bool includeRequests = false)
        {
            _logger.LogInformation($"Selecting active credit partner by token: {id}");

            var res = await GetQuerryCreaditPartnerByFilterFunction(d => d.IdNumber == id, includeRequests)
                 .FirstOrDefaultAsync(d => d.EndDate == null || d.EndDate > DateTime.Now);

            _logger.LogInformation($"Found result: {res is not null}");

            return res;
        }

        //used in case i wanted to filter the data by something else but not repeat general querry operations
        private IQueryable<CreditPartner> GetQuerryCreaditPartnerByFilterFunction(
            System.Linq.Expressions.Expression<Func<CreditPartner, bool>> filter,
            bool includeRequests = false)
        {
            var querry = _context.CreditPartners.AsQueryable();
            return querry.Where(filter);
        }
    }
}
