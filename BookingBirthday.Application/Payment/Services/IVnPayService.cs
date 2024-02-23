using BookingBirthday.Application.Payment.Models;
using Microsoft.AspNetCore.Http;

namespace BookingBirthday.Application.Payment.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}