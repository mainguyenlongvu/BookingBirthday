using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Http;

namespace BookingBirthday.Server.Services;
public interface IVnPayService
{
    string CreateDepositPaymentUrl(int bookingId, PaymentInformationModel model, HttpContext context);
    string CreateRemainingPaymentUrl(int bookingId, PaymentInformationModel model, HttpContext context);
    PaymentResponseModel DepositPaymentExecute(IQueryCollection collections);
    PaymentResponseModel RemainingPaymentExecute(IQueryCollection collections);
}