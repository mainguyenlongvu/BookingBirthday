namespace BookingBirthday.Server.Models;

public class PaymentInformationModel
{
    public string OrderType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
}