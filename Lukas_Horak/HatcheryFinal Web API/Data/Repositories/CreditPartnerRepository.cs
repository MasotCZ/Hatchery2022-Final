using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HatcheryFinal_Web_API.Data
{
    class CreditPartnerRepository : RepositoryBase<CreditPartner>, ICreditPartnerRepository
    {
        public CreditPartnerRepository(BankDbContext context, ILogger<CreditRequestRepository> logger) : base(context, logger)
        {
        }

        public async Task<CreditPartner> GetActiveCreditPartnerByTokenAsync(Guid token)
        {
            _logger.LogInformation($"Selecting active credit partner by token: {token}");

            var res = await GetQuerryCreaditPartnerByToken(token)
                 .FirstOrDefaultAsync(d => d.EndDate == null || d.EndDate > DateTime.Now);

            _logger.LogInformation($"Found result: {res is not null}");

            return res;
        }

        public async Task<CreditPartner> GetCreditPartnerByTokenAsync(Guid token)
        {
            _logger.LogInformation($"Selecting credit partner by token: {token}");

            var res = await GetQuerryCreaditPartnerByToken(token).FirstOrDefaultAsync();

            _logger.LogInformation($"Found result: {res is not null}");

            return res;
        }

        public async Task<CreditPartner> GetInactiveCreditPartnerByTokenASync(Guid token)
        {
            _logger.LogInformation($"Selecting inactive credit partner by token: {token}");

            var res = await GetQuerryCreaditPartnerByToken(token)
                .FirstOrDefaultAsync(d => d.EndDate != null && d.EndDate < DateTime.Now);

            _logger.LogInformation($"Found result: {res is not null}");

            return res;
        }

        private IQueryable<CreditPartner> GetQuerryCreaditPartnerByToken(Guid token)
        {
            var querry = _context.CreditPartners.AsQueryable();
            return querry.Where(d => d.Token == token);
        }
    }
}
