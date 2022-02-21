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
                var requests = await _requestRepository.GetAllUnfulfilledCreditRequestsAsync();

                return Ok(_mapper.Map<CreditRequestDto>(requests));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CreditRequest[]>> Put(int id, [FromBody] CreditRequestDto creditRequestDto)
        {
            try
            {
                var current = await _requestRepository.GetCreditRequestById(id);

                if (current is null)
                {
                    return NotFound();
                }

                current = _mapper.Map(creditRequestDto, current);

                if (await _requestRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok(_mapper.Map<CreditRequestDto>(current));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }
    }
}
