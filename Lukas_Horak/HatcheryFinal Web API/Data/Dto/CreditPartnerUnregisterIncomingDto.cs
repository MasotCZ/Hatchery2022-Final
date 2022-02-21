using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditPartnerUnregisterIncomingDto
    {
        [Required]
        public DateTime EndDate { get; set; }
    }
}
