namespace BookingBirthday.Server.Models;

public class PaymentResponseModel
{
    public string OrderDescription { get; set; }
    public string BookingId { get; set; }
    public string PaymentId { get; set; }
    public bool Success { get; set; }
    public string Token { get; set; }
    public string VnPayResponseCode { get; set; }
    public double Amount {  get; set; }
}