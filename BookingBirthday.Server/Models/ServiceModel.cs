namespace BookingBirthday.Server.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }
        public string? image_url { get; set; }
        public IFormFile? file { get; set; }

        //PromotionId
        public int? PromotionId { get; set; }
    }
}
