using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_Programacion3.Models
{
    public class Client
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre tiene que ser diferente vacio")]
        public string? name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "El email no es valido")]
        public string? email { get; set; }
    }
}
