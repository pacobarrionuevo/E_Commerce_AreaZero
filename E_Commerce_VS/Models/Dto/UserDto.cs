namespace E_Commerce_VS.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Direccion { get; set; }
        public bool esAdmin { get; set; }

    }
}
