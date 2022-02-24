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

        /// <summary>
        /// Used to gather all unfulfilled credit requests for call center
        /// </summary>
        /// <returns>
        /// <see cref="OkObjectResult"/> with all unfulfilled credit requests in format <see cref="CreditRequestOutgoingWithIdDto"/>.
        /// <see cref="ObjectResult"/> with status code  500 otherwise.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<CreditRequestOutgoingWithIdDto[]>> Get()
        {
            try
            {
                var requests = await _requestRepository.GetAllUnfulfilledActiveCreditRequestsAsync();

                return Ok(_mapper.Map<CreditRequestOutgoingWithIdDto[]>(requests));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        /// <summary>
        /// Creates a new credit request in the system by using the data from <see cref="CreditRequestNewIncomingDto"/>
        /// </summary>
        /// <param name="creditRequestDto">Json with all required attributes to form a new credit request <see cref="CreditRequestNewIncomingDto"/></param>
        /// <returns>
        /// <see cref="CreatedResult"/> Created credit request with information gathered from <see cref="creditRequestDto"/>.
        /// <see cref="BadRequestObjectResult"/> if the partner was not registered or inactive or the Db save failed.
        /// <see cref="ObjectResult"/> with status code  500 otherwise.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreditRequestDto>> Post([FromBody] CreditRequestNewIncomingDto creditRequestDto)
        {
            try
            {
                var current = await _partnerRepository.GetActiveCreditPartnerByTokenAsync(creditRequestDto.Token);

                if (current is null)
                {
                    return BadRequest("Partner not registered or inactive");
                }

                //problem mapper spadne pokud contact status neni jedna z enum hodnot
                //nalezeno pred prednaskou
                if (creditRequestDto.ContactStatus is not null 
                    && !Enum.GetNames<CreditRequestStatusCode>().Contains(creditRequestDto.ContactStatus.StatusCode))
                {
                    return BadRequest("Wrong contact status code supplied");
                }

                var request = _mapper.Map<CreditRequest>(creditRequestDto);

                if (request.ContactStatus is null)
                {
                    request.ContactStatus = new CreditRequestStatus() { ContactNotes = "", StatusCode = CreditRequestStatusCode.Unfulfilled };
                }

                _requestRepository.Add(request);

                if (await _requestRepository.SaveChangesAsync() != 2)
                {
                    return BadRequest("Could not save to Db");
                }

                return Created($"api/CreditRequest", _mapper.Map<CreditRequestDto>(request));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        /// <summary>
        /// Changes contact status of a credit request
        /// </summary>
        /// <param name="id">id of the credit request</param>
        /// <param name="creditRequestDto">Json object with all neccesarry information
        /// to change credit request contact status <see cref="CreditRequestStatusChangeIncomingDto"/> for more info</param>
        /// <returns>
        /// <see cref="OkObjectResult"/> with full information about the credit request that was changed <see cref="CreditRequestDto"/>.
        /// <see cref="BadRequestObjectResult"/> or <see cref="ObjectResult"/> with status code  500 otherwise.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CreditRequestDto>> Put(int id, [FromBody] CreditRequestStatusChangeIncomingDto creditRequestDto)
        {
            try
            {
                var current = await _requestRepository.GetCreditRequestByIdAsync(id);

                if (current is null)
                {
                    return NotFound();
                }

                //problem mapper spadne pokud contact status neni jedna z enum hodnot
                //nalezeno pred prednaskou
                if (creditRequestDto.ContactStatus is not null
                    && !Enum.GetNames<CreditRequestStatusCode>().Contains(creditRequestDto.ContactStatus.StatusCode))
                {
                    return BadRequest("Wrong contact status code supplied");
                }

                current = _mapper.Map(creditRequestDto, current);

                if (await _requestRepository.SaveChangesAsync() > 1)
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
