using System.ComponentModel.DataAnnotations;

namespace BookingBirthday.Server.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới", AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Mật khẩu mới phải từ 8 đến 30 ký tự")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mã đặt lại là bắt buộc")]
        public string ResetCode { get; set; }
    }
}
