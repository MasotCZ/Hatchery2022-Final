using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto that is returned on a new partner registration.
    /// </summary>
    public class CreditPartnerRegisteredDto
    {
        /// <summary>
        /// Token that is affiliated with the partner.
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// File that was attached by the partner.
        /// </summary>
        public byte[]? File { get; set; }
    }
}
