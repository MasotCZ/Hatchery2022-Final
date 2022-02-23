using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto used by call center when gathering information about credit requests.
    /// See <see cref="CreditRequestDto"/> for additional attributes used.
    /// </summary>
    public class CreditRequestOutgoingWithIdDto : CreditRequestDto
    {
        /// <summary>
        /// Id of the credit request that should be used to manipulate the credit request later
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
