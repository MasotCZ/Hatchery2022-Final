using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditPartnerChangeEndDateIncomingDto
    {
        [Required]
        public DateTime EndDate { get; set; }
    }
}
