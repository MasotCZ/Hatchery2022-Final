using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditPartnerRegisteredDto
    {
        [Required]
        public string Token { get; set; }
        public byte[]? File { get; set; }
    }
}
