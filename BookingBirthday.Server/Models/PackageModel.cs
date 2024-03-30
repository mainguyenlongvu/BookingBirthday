using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class PackageModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Host_name { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public string Note { get; set; }
        public string? image_url { get; set; }
        public IFormFile? file { get; set; }
        public string? Status { get; set; }
        public bool HasRated { get; set; }
        public string? Gender { get; set; }
        public string Age { get; set; }
        public string PackageType { get; set; }

        //PromotionId
        public int UserId { get; set; }
        public User? User { get; set; }
        public int ThemeId { get; set; }
        public Theme Theme { get; set; }
        public List<PackageLocation> PackageLocations { get; set; }
    }
}
