using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class RegisterRequest
    {
        [Required]
        [Remote(action: "VerifyLogin", controller: "Register", ErrorMessage = "Логин уже используется")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Remote(action: "VerifyEmail", controller: "Register", ErrorMessage = "E-mail уже используется")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
