using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Models.AccountModel
{
    public class SignView
    {
        public RegisterViewModel RegisterViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public SignView()
        {
            RegisterViewModel = new RegisterViewModel();
            LoginViewModel = new LoginViewModel();
        }
    }
}
