using Microsoft.EntityFrameworkCore;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    public class UserRepository: IUserRepository
    {
        // ссылка на контекст
        private readonly ApplicationContext _context;
        private UserRole admin = new UserRole(1, "Admin");
        private UserRole common = new UserRole(2, "User");
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public void GetContext()
        {

        }

        public async Task CreateUser(User user)
        {
            user.RoleId = common.Id;

            var findUserEmail = _context.Users.Any(e=>e.Email == user.Email);
            var indUserLogin = _context.Users.Any(u=>u.Login == user.Login);

            if (!findUserEmail && !indUserLogin)
            {
                await _context.Users.AddAsync(user);
                _context.SaveChanges();
            }
            else { Console.WriteLine("User id {0} is exist", user.Id); return; }
        }
        public async Task<User> FindUserById(Guid id)
        {
           User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
           if(user !=null)
           return user;

            return null;
        }

        public async void DeleteUser(Guid id)
        {
            var deletedUser = FindUserById(id);
            if (deletedUser != null)
                _context.Users.Remove(await deletedUser);
        }
        public void UpdateUser(User user)
        {
            var userToUpdate =  FindUserById(user.Id);
            if (userToUpdate != null)
            _context.Update(userToUpdate);
            _context.SaveChanges();
        }
        public User FindByEmail(string email)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null) return user;

            return null;
        }
        public ICollection<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }
    }

    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task<User> FindUserById(Guid id);
        void DeleteUser(Guid id);
        void UpdateUser(User user);
        User FindByEmail(string email);
    }
}
