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
        private readonly ICreditPartnerRepository _partnerRepository;
        private readonly IMapper _mapper;

        public CreditRequestController(ICreditRequestRepository requestRepository, ICreditPartnerRepository partnerRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _partnerRepository = partnerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CreditRequestDto[]>> Get()
        {
            try
            {
                var requests = await _requestRepository.GetAllUnfulfilledActiveCreditRequestsAsync();

                return Ok(_mapper.Map<CreditRequestDto[]>(requests));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreditRequestDto>> Post([FromBody] CreditRequestDto creditRequestDto)
        {
            try
            {
                var current = await _partnerRepository.GetActiveCreditPartnerByTokenAsync(creditRequestDto.Token);

                if (current is null)
                {
                    return BadRequest("Partner not registered or inactive");
                }

                var request = _mapper.Map<CreditRequest>(creditRequestDto);

                _requestRepository.Add(request);

                if (await _requestRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok(_mapper.Map<CreditRequestDto>(request));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CreditRequestStatusChangeIncomingDto>> Put(int id, [FromBody] CreditRequestStatusChangeIncomingDto creditRequestDto)
        {
            try
            {
                var current = await _requestRepository.GetCreditRequestByIdAsync(id);

                if (current is null)
                {
                    return NotFound();
                }

                current = _mapper.Map(creditRequestDto, current);

                if (await _requestRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok(_mapper.Map<CreditRequestStatusChangeIncomingDto>(current));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }
    }
}
