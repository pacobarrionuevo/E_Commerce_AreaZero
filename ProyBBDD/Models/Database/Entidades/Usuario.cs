using System.ComponentModel.DataAnnotations;

namespace E_Commerce_VS.Models.Database.Entidades
{
    public class Usuario
    {

        public int UsuarioId { get; set; }

        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string Direccion { get; set; }

        public string? Rol { get; set; } 

    }
}
