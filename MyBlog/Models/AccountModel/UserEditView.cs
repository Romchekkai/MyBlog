using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.AccountModel
{
    public class UserEditView
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [Remote(action: "VerifyLogin", controller: "AccountManager", ErrorMessage = "Логин используется")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [Remote(action: "VerifyEmail", controller: "AccountManager", ErrorMessage = "Email уже используется")]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
        public DateTime DateOfTheBirth { get; set; }
        public string? Image { get; set; }
        public string Status { get; set; }
        public string? About { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
