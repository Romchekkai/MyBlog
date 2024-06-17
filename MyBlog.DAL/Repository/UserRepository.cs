﻿using Microsoft.EntityFrameworkCore;
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
        private UserRole moderator = new UserRole(3,"Moderator");

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public void GetContext()
        {

        }

        public async Task CreateUser(UserEntity user)
        {
            user.RoleId = common.Id;

            var findUserEmail = _context.Users.Any(e=>e.Email == user.Email);
            var findUserLogin = _context.Users.Any(u=>u.Login == user.Login);

            if (!findUserEmail && !findUserLogin)
            {
                await _context.Users.AddAsync(user);
                _context.SaveChanges();
            }
            else { Console.WriteLine("User id {0} exists", user.Id); return; }
        }
        public async Task<UserEntity> FindUserById(Guid id)
        {
           UserEntity? user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Id == id);
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
        public async Task UpdateUser(UserEntity user)
        {
            var userToUpdate =  await FindUserById(user.Id);
            if (userToUpdate != null)
            _context.Update(userToUpdate);
            _context.SaveChanges();
        }
        public async Task<UserEntity> FindByLogin(string login)
        {
            UserEntity? user =  await _context.Users.Include(r=>r.Role).FirstOrDefaultAsync(u => u.Login == login);
            if(user is null) return null;

            return user;
        }
        public async Task<UserEntity> FindByEmail(string email)
        {
            UserEntity? user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) return null;

            return user;
        }
        public async Task<UserEntity> FindByID(Guid id)
        {
            UserEntity? user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(i => i.Id == id);
            if(user is null) return null;

            return user;
        }

        public async Task<bool> CheckByEmail(string email)
        {
            bool check = await _context.Users.AnyAsync(u => u.Email == email);
            return check;
        }

        public async Task<bool> CheckByLogin(string login)
        {
            bool check = await _context.Users.AnyAsync(l => l.Login == login);
            return check;
        }
        public async Task<bool> CheckByPassword(string password, string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(l => l.Login == login);
            if (user is null) return false;
            bool check = password == user.Password;

            return check;
        }
        
        public async Task<IEnumerable<UserEntity>> GetAllUsers()
        {
            var users = await _context.Users.Include(r=>r.Role).ToListAsync();
            return users;
        }

        public async Task<IEnumerable<UserRole>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }

        public async Task ChangeRole(int role, Guid userID)
        {
            var user = await FindByID(userID);
            if(user is null) return;
            user.RoleId = role;

            _context.Update(user);
            _context.SaveChanges();
        }
        

    }

    public interface IUserRepository
    {
        Task CreateUser(UserEntity user);
        Task<UserEntity> FindUserById(Guid id);
        void DeleteUser(Guid id);
        Task UpdateUser(UserEntity user);
        Task<UserEntity> FindByLogin(string login);
        Task<UserEntity> FindByEmail(string email);
        Task<bool> CheckByPassword(string password, string login);
        Task<UserEntity> FindByID(Guid id);
        Task<bool> CheckByEmail(string email);
        Task<bool> CheckByLogin(string login);
        Task<IEnumerable<UserEntity>> GetAllUsers();
        Task<IEnumerable<UserRole>> GetRoles();
        Task ChangeRole(int role, Guid userID);
    }
}
