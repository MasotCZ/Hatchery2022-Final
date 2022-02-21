using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditPartnerFullInfoDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Id muset be higher than {0}")]
        public int IdNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public byte[]? File { get; set; }
    }

    public class CreditRequestDto
    {
        [Required]
        [Range(20_000, 500_000)]
        public decimal Credit { get; set; }

        [Required]
        [Range(6, 96)]
        public int MonthsTillCreditMaturity { get; set; }

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

        public CreditRequestStatus ContactStatus { get; set; } = CreditRequestStatus.Default;
    }
}
