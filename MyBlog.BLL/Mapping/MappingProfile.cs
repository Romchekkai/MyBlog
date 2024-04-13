using AutoMapper;
using MyBlog.BLL.Models;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
        }
    }
}
//ForMember(m=>m.ModelRole, opt=>opt.MapFrom(scr=>scr.Role));