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
}
