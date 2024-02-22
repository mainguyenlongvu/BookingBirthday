namespace BookingBirthday.Server.Models
{
    public class PackageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }
        public string? image_url { get; set; }
        public IFormFile? file { get; set; }

        //PromotionId
        public int? PromotionId { get; set; }

        public int? ServiceId { get; set; }
        public string? Service_Name { get; set; }

    }
}
