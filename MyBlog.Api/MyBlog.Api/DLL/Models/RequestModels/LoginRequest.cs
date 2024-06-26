using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class LoginRequest
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required]
        [Display(Name = "Login")]
        [Remote(action: "VerifyForLogin", controller: "Register", ErrorMessage = "Логин не найден")]
        public string Login { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        // [Remote(action: "VerifyPassword", controller: "Register", ErrorMessage = "Пароль не совпадает")] 
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
