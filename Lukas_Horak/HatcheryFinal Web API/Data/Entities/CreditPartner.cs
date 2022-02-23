using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatcheryFinal_Web_API.Data.Entities
{
    //on Create, send back Token
    public class CreditPartner
    {
        private int _idNumber;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(0, int.MaxValue, ErrorMessage = "Id muset be higher than {0}")]
        public int IdNumber
        {
            get => _idNumber;
            set
            {
                _idNumber = value;

                if (Token is default(string) || Token is null)
                {
                    Token = generateToken();
                }
            }
        }

        [Required]
        public string Token
        {
            get;
            set;
        }

        private string generateToken()
        {
            const int SALT = 654651361;
            return HashCode.Combine(IdNumber, SALT).ToString();
        }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public byte[]? File { get; set; }

        public ICollection<CreditRequest>? Requests { get; set; }
    }
}
