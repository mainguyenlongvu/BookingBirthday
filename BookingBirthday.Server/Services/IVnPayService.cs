using BookingBirthday.Server.Models;
using Microsoft.AspNetCore.Http;

namespace BookingBirthday.Server.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}