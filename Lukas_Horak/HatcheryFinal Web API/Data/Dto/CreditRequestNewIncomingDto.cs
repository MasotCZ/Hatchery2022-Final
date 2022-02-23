using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto used when registering new credit requests by partners.
    /// See <see cref="CreditRequestDto"/> for additional attributes used.
    /// </summary>
    public class CreditRequestNewIncomingDto : CreditRequestDto
    {
        /// <summary>
        /// Token of the partner that registered this credit request.
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
