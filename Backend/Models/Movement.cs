using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_Programacion3.Models
{
    public class Movement
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int? clientID { get; set; }

        [ForeignKey("clientID")]
        public Client? client { get; set; }

        [Required]
        public string? action { get; set; }

        [Required]
        public string? cryptoCode { get; set; }

        [Required]
        public decimal? cryptoAmount { get; set; }

        [Required]
        public decimal? total { get; set; }

        [Required]
        public DateTime? date { get; set; }
    }
}