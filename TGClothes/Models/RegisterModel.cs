using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class RegisterModel
    {
        [Key]
        public long Id { get; set; }

        [Display(Name ="Tên người dùng")]
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$", ErrorMessage = "Địa chỉ email không hợp lệ, vui lòng nhập lại")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Mời nhập số điện thoại")]
        [RegularExpression(@"^(0[1-9]\d{8}|0[1-9]\d{9})$", ErrorMessage = "Số điện thoại không hợp lệ, vui lòng nhập lại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Mời nhập địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(20, MinimumLength = 6,  ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage ="Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }
    }
}