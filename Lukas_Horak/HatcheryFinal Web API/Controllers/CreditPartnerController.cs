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
    public class CreditPartnerController : ControllerBase
    {
        private readonly ICreditPartnerRepository _partnerRepository;
        private readonly IMapper _mapper;

        public CreditPartnerController(ICreditPartnerRepository partnerRepository, IMapper mapper)
        {
            _partnerRepository = partnerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Method that registers a new partner into the system, for inactive partners please use <see cref="Put"/>
        /// </summary>
        /// <param name="creditPartnerDto">Input json object <see cref="CreditPartnerFullInfoDto"/></param>
        /// <returns>
        /// OkObjectResult with generated Token and information about the partner unless the partner was already registered or inactive,
        /// in which case the method will check if the parter is inactive, if it is then it will change the partner data to the input data.
        /// If the partner is registered and active BadRequestObjectResult will be returned.
        /// BadRequestObjectResult or ObjectResult with status code  500 otherwise.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CreditPartnerRegisteredDto>> Post([FromBody] CreditPartnerFullInfoDto creditPartnerDto)
        {
            try
            {
                var current = await _partnerRepository.GetCreditPartnerByIdAsync(creditPartnerDto.IdNumber);

                CreditPartner toAdd;
                if (current is null)
                {
                    //new partner
                    toAdd = _mapper.Map<CreditPartner>(creditPartnerDto);
                    _partnerRepository.Add(toAdd);
                }
                else
                {
                    if (current.EndDate is null || current.EndDate > DateTime.Now)
                    {
                        //active partner
                        return BadRequest("Partner already registered");
                    }

                    //updating registered but inactive partner
                    toAdd = _mapper.Map(creditPartnerDto, current);
                }

                if (await _partnerRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok(_mapper.Map<CreditPartnerRegisteredDto>(toAdd));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        /// <summary>
        /// Method that changes a partners contract end date
        /// </summary>
        /// <param name="token">Registered token of the partner</param>
        /// <param name="creditPartnerUnregisterDto">Input json object <see cref="CreditPartnerChangeEndDateIncomingDto"/></param>
        /// <returns>
        /// OkObjectResult with partner data.
        /// NotFoundObjectResult if the token does not exist.
        /// BadRequestObjectResult or ObjectResult with status code  500 otherwise.
        /// </returns>
        [HttpPut("{token}")]
        public async Task<ActionResult<CreditPartnerFullInfoDto>> Put(string token, [FromBody] CreditPartnerChangeEndDateIncomingDto creditPartnerUnregisterDto)
        {
            try
            {
                var current = await _partnerRepository.GetCreditPartnerByTokenAsync(token);

                if (current is null)
                {
                    return NotFound();
                }

                current = _mapper.Map(creditPartnerUnregisterDto, current);

                if (await _partnerRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok(_mapper.Map<CreditPartnerFullInfoDto>(current));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        /// <summary>
        /// Deletes a partner from the system.
        /// Should only be used internally
        /// </summary>
        /// <param name="id">id of the partner to delete</param>
        /// <returns>
        /// OkObjectResult on succesfull delete.
        /// NotFoundObjectResult if not partner exists with id.
        /// BadRequestObjectResult or ObjectResult with status code  500 otherwise.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var current = await _partnerRepository.GetCreditPartnerByIdAsync(id);

                if (current is null)
                {
                    return NotFound();
                }

                _partnerRepository.Remove(current);

                if (await _partnerRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }
    }
}
