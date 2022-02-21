using AutoMapper;
using HatcheryFinal_Web_API.Data.Entities;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HatcheryFinal_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditRequestController : ControllerBase
    {
        private readonly ICreditRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public CreditRequestController(ICreditRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CreditRequest[]>> Get()
        {
            try
            {
                var requests = _requestRepository.GetAllUnfulfilledCreditRequestsAsync();

                return Ok(_mapper.Map<CreditRequestDto>(requests));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }
    }
}
