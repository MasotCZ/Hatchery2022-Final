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
        /// TODO
        /// </summary>
        /// <param name="creditPartnerRegisterDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreditPartner>> Post([FromBody] CreditPartnerFullInfoDto creditPartnerRegisterDto)
        {
            try
            {
                var toAdd = _mapper.Map<CreditPartner>(creditPartnerRegisterDto);
                _partnerRepository.Add(toAdd);

                if (await _partnerRepository.SaveChangesAsync() != 1)
                {
                    return BadRequest("Could not save to Db");
                }

                return Ok(_mapper.Map<CreditPartnerFullInfoDto>(toAdd));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database fail {e.Message}");
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="creditPartnerUnregisterDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CreditPartner>> Put(int id, [FromBody] CreditPartnerUnregisterIncomingDto creditPartnerUnregisterDto)
        {
            try
            {
                var current = await _partnerRepository.GetCreditPartnerByIdAsync(id);

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
        /// TODO
        /// In case i need to delete partner, bacause error or smth, shouldnt be used really unless error
        /// </summary>
        /// <param name="creditPartnerDto"></param>
        /// <returns></returns>
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
