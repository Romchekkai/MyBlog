using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Models.AccountModel
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "VerifyLogin", controller: "Register", ErrorMessage = "Email уже используется")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [Remote(action: "VerifyEmail", controller: "Register", ErrorMessage = "Email уже используется")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
        public string? PasswordConfirm { get; set; }

    }
}
