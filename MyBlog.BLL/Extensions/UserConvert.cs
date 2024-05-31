using MyBlog.BLL.Models.UserModels;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Extensions
{
    public static class UserConvert
    {
        public static UserEntity Convert(this UserEntity user, UserModel model)
        {
           // user.Id = model.Id;
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

