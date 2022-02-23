using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto to provide contact status for a credit request used by Call center to modify unfulfilled requests.
    /// </summary>
    public class CreditRequestStatusChangeIncomingDto
    {
        /// <summary>
        /// Contact status information that will replace contact status on credit request.
        /// </summary>
        [Required]
        public CreditRequestStatusDto ContactStatus { get; set; }
    }
}
