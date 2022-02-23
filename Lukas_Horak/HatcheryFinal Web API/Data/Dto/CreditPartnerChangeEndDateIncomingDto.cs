using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto used to change end date of partners contract.
    /// </summary>
    public class CreditPartnerChangeEndDateIncomingDto
    {
        /// <summary>
        /// Contract end date to change to.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
