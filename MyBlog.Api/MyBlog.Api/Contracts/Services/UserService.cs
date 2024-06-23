using AutoMapper;
using MyBlog.Api.Contracts.Extensions;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;
using MyBlog.Api.DLL.Repositories;
using System.Text;

namespace MyBlog.Api.Contracts.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task CreateUser(RegisterRequest model)
        {
            var user = _mapper.Map<User>(model);
            await _userRepo.CreateUser(user);
        }
        public async Task<User> EditUser(UserEditRequest model)
        {
            var foundedUser = await _userRepo.FindUserById(model.Id);
            foundedUser.Convert(model);

            await _userRepo.UpdateUser(foundedUser);
        
            return foundedUser;
        }

        public async Task<User> FindUserById(Guid id)
        {
            var foundedUser = await _userRepo.FindUserById(id);
            return foundedUser;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            return users;
        }

        public async Task<User> FindUserByLogin(string login)
        {
            var foundedUser = await _userRepo.FindByLogin(login);
            return foundedUser;
        }
        public async Task<User> FindUserByEmail(string email)
        {
            var foundedUser = await _userRepo.FindByEmail(email);
            return foundedUser;
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
      /*  public async Task<bool> CheckByPassword(string password, string login)
        {
            bool verify = await _userRepo.CheckByPassword(password, login);
            return verify;
        }*/

      /*  public async Task<IEnumerable<UserRoleModel>> GetRoles()
        {
            var roles = await _userRepo.GetRoles();
            var rolesModel = _mapper.Map<IEnumerable<UserRoleModel>>(roles);

            return rolesModel;
        }*/

        public async Task ChangeRole(int role, Guid userID)
        {
            await _userRepo.ChangeRole(role, userID);
        }


    }
    public interface IUserService
    {
        Task CreateUser(RegisterRequest model);
        Task<User> EditUser(UserEditRequest model);
        Task<User> FindUserById(Guid id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> FindUserByLogin(string login);
        Task<User> FindUserByEmail(string email);
        Task<bool> CheckByEmail(string email);
        Task<bool> CheckByLogin(string login);
        Task ChangeRole(int role, Guid userID);
    }

}
