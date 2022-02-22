using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditRequestStatusChangeIncomingDto
    {
        [Required]
        public CreditRequestStatusDto? ContactStatus { get; set; }
    }
}
