namespace BookingBirthday.Server.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
