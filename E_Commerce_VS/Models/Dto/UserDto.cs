namespace E_Commerce_VS.Models.Dto
{
    public class UserDto
    {
        //name adress password email
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Direccion { get; set; }
        public string Rol { get; set; }
        public bool EsAdmin { get; set; }

    }
}
