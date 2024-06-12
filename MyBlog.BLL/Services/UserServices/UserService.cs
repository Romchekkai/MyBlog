using AutoMapper;
using MyBlog.BLL.Extensions;
using MyBlog.BLL.Models.UserModels;
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

        public async Task CreateUser(UserModel model)
        {
            var user = _mapper.Map<UserEntity>(model);
            await _userRepo.CreateUser(user);
        }

        public async Task<UserModel> FindUserByLogin(string login)
        {
            var foundedUser =  await _userRepo.FindByLogin(login);
            var userModel = _mapper.Map<UserModel>(foundedUser);
            return userModel;
        }

        public async Task<UserModel> FindUserById(Guid id)
        {
            var foundedUser = await _userRepo.FindUserById(id);
            var userModel = _mapper.Map<UserModel>(foundedUser);
            return userModel;
        }
        public async Task<UserModel> FindUserByEmail(string email)
        {
            var foundedUser = await _userRepo.FindByEmail(email);
            var userModel = _mapper.Map<UserModel>(foundedUser);
            return userModel;
        }


        public async Task<bool> CheckByEmail(string email)
        {
            bool verify = await _userRepo.CheckByEmail(email);
            return verify;
        }
        public async Task<bool> CheckByLogin(string login)
        {
            bool verify = await _userRepo.CheckByLogin(login);
            return verify;
        }


        public async Task<UserModel> UpdateUser(UserModel model)
        {
            var foundedUser = await _userRepo.FindUserById(model.Id);
            foundedUser.Convert(model);

            await _userRepo.UpdateUser(foundedUser);
            return model;
        }
    }
    public interface IUserService
    {
        Task<UserModel> UpdateUser(UserModel model);
        Task CreateUser(UserModel model);
        Task<UserModel> FindUserByLogin(string login);
        Task<UserModel> FindUserByEmail(string email);
        Task<UserModel> FindUserById(Guid id);
        Task<bool> CheckByEmail(string email);
        Task<bool> CheckByLogin(string login);
    }

}
