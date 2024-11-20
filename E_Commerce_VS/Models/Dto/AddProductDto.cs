namespace E_Commerce_VS.Models.Dto
{
    public class AddProductDto
    {
        public long ProductId { get; set; }
        public int? UserId { get; set; }
        public int Quantity { get; set; }
    }

}
