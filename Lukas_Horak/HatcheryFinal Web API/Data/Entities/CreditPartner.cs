﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatcheryFinal_Web_API.Data.Entities
{
    //on Create, send back Token
    public class CreditPartner
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public Guid Token { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Id muset be higher than {0}")]
        public int IdNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public byte[]? File { get; set; }

        [ForeignKey("Token")]
        public ICollection<CreditRequest>? Requests { get; set; }
    }
}