using AutoMapper;
using HatcheryFinal_Web_API.Data.Dto;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HatcheryFinal_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerReportController : ControllerBase
    {

        private readonly IProfitabilityRepository _repository;
        private readonly IMapper _mapper;

        public PartnerReportController(IProfitabilityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a report of profits and shows the most profitable one.
        /// </summary>
        /// <returns>
        /// <see cref="OkObjectResult"/> with dto report of most profitable partner, where profits are calculated by total accepted credit.
        /// <see cref="NotFoundObjectResult"/> when no partner with profit is found.
        /// <see cref="ObjectResult"/> with status code  500 otherwise.
        /// </returns>
        [Route("MostProfitable")]
        [HttpGet]
        public async Task<ActionResult<ProfitabilityReportDto>> MostProfitablePartner()
        {
            try
            {
                var current = await _repository.GetMostProfitablePartnerAsync(true);

                if (current is null || current.Requests is null)
                {
                    return NotFound("No profits detected");
                }

                //save all requests for later
                var allRequests = current.Requests;

                //filter out requests that are not finished
                current.Requests = current.Requests.Where(d => d.ContactStatus.StatusCode == CreditRequestStatusCode.Accepted).ToArray();

                if (current.Requests is null || current.Requests.Count == 0)
                {
                    return NotFound("No profits detected");
                }

                var res = new ProfitabilityReportDto()
                {
                    Partner = _mapper.Map<CreditPartnerFullInfoDto>(current),
                    TotalCredit = current.Requests.Sum(d => d.Credit),
                    SuccessRate = (decimal)current.Requests.Count / allRequests.Count
                };

                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        /// <summary>
        /// Creates a report of profits and shows the most successful partner.
        /// </summary>
        /// <returns>
        /// <see cref="OkObjectResult"/> with Most successful partner where success is calculated as contact success rate.
        /// <see cref="NotFoundObjectResult"/> when no partner with profit is found.
        /// <see cref="ObjectResult"/> with status code  500 otherwise.
        /// </returns>
        [Route("MostSuccessful")]
        [HttpGet]
        public async Task<ActionResult<ProfitabilityReportDto>> MostSuccessfulPartner()
        {
            try
            {
                var current = await _repository.GetMostSuccessfulPartnerAsync(true);

                if (current is null || current.Requests is null)
                {
                    return NotFound("No profits detected");
                }

                //save all requests for later
                var allRequests = current.Requests;

                //filter out requests that are not finished
                current.Requests = current.Requests.Where(d => d.ContactStatus.StatusCode == CreditRequestStatusCode.Accepted).ToArray();

                if (current.Requests is null || current.Requests.Count == 0)
                {
                    return NotFound("No profits detected");
                }

                var res = new ProfitabilityReportDto()
                {
                    Partner = _mapper.Map<CreditPartnerFullInfoDto>(current),
                    TotalCredit = current.Requests.Sum(d => d.Credit),
                    SuccessRate = (decimal)current.Requests.Count / allRequests.Count
                };

                return Ok(res);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }
    }
}
