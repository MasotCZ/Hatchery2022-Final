using HatcheryFinal_Web_API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace HatcheryFinal_Web_API.Data.Dto
{
    public class CreditRequestStatusDto
    {
        public CreditRequestStatusCode StatusCode { get; set; }

        [StringLength(400)]
        public string? ContactNotes { get; set; }
    }
}
