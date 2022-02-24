using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Entities
{
    public enum CreditRequestStatusCode
    {
        Unfulfilled,
        Accepted,
        CouldntContact,
        Refused,
    }

    public class CreditRequestStatus
    {
        [Key]
        public int Id { get; set; }

        public CreditRequestStatusCode StatusCode { get; set; }

        [StringLength(512)]
        public string? ContactNotes { get; set; }
    }
}
