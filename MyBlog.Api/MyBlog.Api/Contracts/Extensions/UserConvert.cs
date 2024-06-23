using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.Contracts.Extensions
{
    public static class UserConvert
    {
        public static User Convert(this User user, UserEditRequest model)
        {
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.Login = model.Login;
            user.Password = model.Password;
            user.DateOfTheBirth = model.DateOfTheBirth;
            user.Image = model.Image;
            user.Status = model.Status;
            user.About = model.About;
            user.PhoneNumber = model.PhoneNumber;

            return user;
        }
    }
}

