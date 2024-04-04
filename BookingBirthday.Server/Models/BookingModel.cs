using BookingBirthday.Data.Entities;

namespace BookingBirthday.Server.Models
{
    public class BookingModel
    {   
        public int Id { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_cancel { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public string? Reason { get; set; }
        public string ChildName { get; set; }
        public DateTime ChildDateOfBirth { get; set; }
        public string ChildGender { get; set; }
        public int ChildNumber { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public string? BookingStatus { get; set; }
        public double Total { get; set; }
        //GuestId
        public int UserId { get; set; }
        //PaymentId
        public int PaymentId { get; set; }
        public List<PackageModel>? PackageModel { get; set; }
    }
}
