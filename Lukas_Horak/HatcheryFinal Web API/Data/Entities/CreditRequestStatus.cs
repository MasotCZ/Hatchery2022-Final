using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Entities
{
    public enum CreditRequestStatusCode
    {
        Unfulfilled,
        CouldntContact,
        Accepted,
        Refused,
    }

    public class CreditRequestStatus
    {
        public static CreditRequestStatus Default = new CreditRequestStatus()
        {
            StatusCode = CreditRequestStatusCode.Unfulfilled,
            ContactNotes = ""
        };

        [Key]
        public int Id { get; set; }

        public CreditRequestStatusCode StatusCode { get; set; }

        [StringLength(512)]
        public string? ContactNotes { get; set; }
    }
}
