﻿    namespace E_Commerce_VS.Models.Dto
    {
        public class UserUpdateDto
        {
            public int UsuarioId { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Direccion { get; set; }
            public bool esAdmin { get; set; }
        }
    }

