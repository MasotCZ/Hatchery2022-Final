using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditRequestDto
    {
        [Required]
        [Range(20_000, 500_000)]
        public decimal Credit { get; set; }

        [Required]
        [Range(6, 96)]
        public int CreditLengthInMonths { get; set; }

        //contact info
        [Required]
        [Phone]
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Surname { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(400)]
        public string? Note { get; set; }

        public CreditRequestStatusDto? ContactStatus { get; set; }
        public CreditPartnerFullInfoDto? PartnerFullInfo { get; set;}
    }
}
