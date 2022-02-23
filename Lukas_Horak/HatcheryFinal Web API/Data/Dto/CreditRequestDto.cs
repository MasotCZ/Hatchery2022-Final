using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    /// <summary>
    /// Dto With all information that is to be shown about credit requests.
    /// See <see cref="CreditRequestNewIncomingDto"/> and <see cref="CreditRequestOutgoingWithIdDto"/> for more specific versions.
    /// </summary>
    public class CreditRequestDto
    {
        /// <summary>
        /// Credit value that is being requested. 
        /// Range(20000, 500000).
        /// </summary>
        [Required]
        [Range(20_000, 500_000)]
        public decimal Credit { get; set; }

        /// <summary>
        /// Time till credit maturity.
        /// Range(6,96).
        /// </summary>
        [Required]
        [Range(6, 96)]
        public int CreditLengthInMonths { get; set; }

        /// <summary>
        /// Phone number of the client.
        /// Max length 64.
        /// </summary>
        [Required]
        [Phone]
        [StringLength(64)]
        public string Phone { get; set; }

        /// <summary>
        /// Name of the client.
        /// Max length 255.
        /// </summary>
        [StringLength(255)]
        public string? Name { get; set; }

        /// <summary>
        /// Surname of the client.
        /// Max length 255.
        /// </summary>
        [StringLength(255)]
        public string? Surname { get; set; }

        /// <summary>
        /// Email of the client.
        /// Max length 255.
        /// </summary>
        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Note about the client.
        /// Max length 512.
        /// </summary>
        [StringLength(512)]
        public string? Note { get; set; }

        /// <summary>
        /// Contact status about the client, if the request was fulfilled or not
        /// </summary>
        public CreditRequestStatusDto? ContactStatus { get; set; }
    }
}
