using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class SignView
    {
        public RegisterRequest RegisterViewModel { get; set; }
        public LoginRequest LoginViewModel { get; set; }
        public SignView()
        {
            RegisterViewModel = new RegisterRequest();
            LoginViewModel = new LoginRequest();
        }
    }
}
