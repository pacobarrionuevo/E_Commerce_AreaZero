namespace E_Commerce_VS.Models.Dto
{
    public class CreateUpdateReviewRequest
    {
        public DateTime FechaPublicacion {  get; set; }
        public string TextReview {  get; set; }
        public uint Label { get; set; }
    }
}
