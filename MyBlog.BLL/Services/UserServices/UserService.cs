using AutoMapper;
using MyBlog.BLL.Models;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Services.UserServices
{
    public class UserService:IUserService
    {
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public UserService(IUserRepository userRepo,IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task CreateUser(UserModel model, string password)
        {
            var user = _mapper.Map<User>(model);
            user.Password = password;
            

           await _userRepo.CreateUser(user);
        }

        public void UpdateUser(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
    public interface IUserService
    {
        void UpdateUser(UserModel model);
        Task CreateUser(UserModel model, string password);
    }

}
