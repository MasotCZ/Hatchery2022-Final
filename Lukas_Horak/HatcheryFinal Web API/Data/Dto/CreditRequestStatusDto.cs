using HatcheryFinal_Web_API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto of credit request status to provide general information about the contact status of a credit request.
    /// </summary>
    public class CreditRequestStatusDto
    {
        /// <summary>
        /// Current status of the credit request converted to string <seealso cref="CreditRequestStatusCode"/>.
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Notes written by call center employee about the contact itself.
        /// <see cref="string"/> of maximum length 512.
        /// </summary>
        [StringLength(512)]
        public string? ContactNotes { get; set; }
    }
}
