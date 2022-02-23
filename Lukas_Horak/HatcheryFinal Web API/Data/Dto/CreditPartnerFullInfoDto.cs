using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto containing all necessary information to create a partner in the system.
    /// </summary>
    public class CreditPartnerFullInfoDto
    {
        /// <summary>
        /// ICO number of the partner.
        /// Is Unique to the partner.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Id muset be higher than {0}")]
        public int IdNumber { get; set; }

        /// <summary>
        /// Name of the partner.
        /// Max length 255.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Date of the contract start.
        /// All request calls will be ignored before this date.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date of the contract end.
        /// All request calls will be ignored after this date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Attached file to this partner.
        /// </summary>
        public byte[]? File { get; set; }
    }
}
