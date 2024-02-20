using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TGClothes.Models
{
    public class CustomerInfo
    {
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Mời nhập họ tên người nhận")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$", ErrorMessage = "Địa chỉ email không hợp lệ, vui lòng nhập lại")]
        [Required(ErrorMessage = "Mời nhập email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Mời nhập số điện thoại")]
        [RegularExpression(@"^(0[1-9]\d{8}|0[1-9]\d{9})$", ErrorMessage = "Số điện thoại không hợp lệ, vui lòng nhập lại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Mời nhập địa chỉ")]
        public string Address { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }
    }
}