namespace BookingBirthday.Server.Models
{
    public class PackageModel
    {
        public int Id { get; set; }
        public string?Name { get; set; }
        public Double Price { get; set; }
        public string? Venue { get; set; }
        public string? Detail { get; set; }
        public string Note { get; set; }
        public string? image_url { get; set; }
        public IFormFile? file { get; set; }
        public string? Status { get; set; }


        //PromotionId
        public int? PromotionId { get; set; }
        public int UserId { get; set; }

    }
}
