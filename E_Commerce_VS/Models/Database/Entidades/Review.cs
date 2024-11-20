    using System.ComponentModel.DataAnnotations;

    namespace E_Commerce_VS.Models.Database.Entidades
    {
        public class Review
        {
            public int Id { get; set; }
            public DateTime FechaPublicacion { get; set; }
            public string TextReview { get; set; }
            public int Label { get; set; }

            public int UsuarioId { get; set; }
            public long ProductoId { get; set; }
        }
    }
