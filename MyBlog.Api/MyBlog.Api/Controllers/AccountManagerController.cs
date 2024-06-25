using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Api.Contracts.Services;
using MyBlog.Api.DLL.Models.RequestModels;
using System.Security.Claims;
using Azure.Core;
using MyBlog.Api.DLL.Models.ResponseModels;

namespace MyBlog.Api.Controllers
{
    public class AccountManagerController : Controller
    {
        private IUserService _userService;

        public AccountManagerController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("Ошибка ввода значения");

            var user = await _userService.FindUserByLogin(model.Login);
            if (user.Password != model.Password)
                throw new InvalidOperationException("Введен неверный пароль");   

                //аутентификация с помощью Cookies
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name )};

                ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
            var timeAuth = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(identity), timeAuth);
            return StatusCode(200);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCode(200); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("createUser")]
        public async Task<IActionResult> CreateUser(RegisterRequest model)
        {
            try
            {
                await _userService.CreateUser(model);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
        [Authorize]
        [Route("userpage")]
        [HttpGet]
        public async Task<User> UserPage()
        {
            var user = User;
            try
            {
              var userResp = await _userService.FindUserByLogin(user.Identity!.Name!);
              return userResp;
            }
            catch
            {
                throw new Exception("Произошла внутренняя ошибка (Get user)");
            }  
        }
        [Authorize]
        [Route("users")]
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            try
            {
                var usersResp = await _userService.GetAllUsers();
                return usersResp;
            }
            catch
            {
                throw new Exception("Произошла внутренняя ошибка (Get users)");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("deleteUser")]
        public async Task<IActionResult> UserDelete(Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(404);
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("editUser")]
        public async Task<IActionResult> UserEdit(UserEditRequest model)
        {
            try
            { 
                var user = await _userService.EditUser(model);

                if (HttpContext.User.Identity is ClaimsIdentity claimsIdentity)
                {
                    if (model.Login != user.Login)
                    {
                        var userLogin = claimsIdentity.FindFirst(ClaimTypes.Name);
                        if (claimsIdentity.TryRemoveClaim(userLogin))
                        {
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(claimsPrincipal);
                        }
                    }
                    if (model.Email != user.Email)
                    {
                        var userEmail = claimsIdentity.FindFirst(ClaimTypes.Email);
                        if (claimsIdentity.TryRemoveClaim(userEmail))
                        {
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(claimsPrincipal);
                        }
                    }
                }
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("editroleuser")]
        public async Task<IActionResult> EditUserRole(RoleEditRequest request)
        {
            try
            {
                await _userService.ChangeRole(request.Id, request.UserId);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(UserEditRequest model)
        {
            bool result = false;
            bool checkEqual = false;
            var foundUser = await _userService.CheckByEmail(model.Email);

            var equal = await _userService.FindUserByEmail(model.Email);
            if (equal != null)
                checkEqual = equal.Email == model.Email;


            if (foundUser && checkEqual)
            {
                result = false;
            }
            else
            {
                result = true;
            }


            if (result)
                return Json(false);
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyLogin(UserEditRequest model)
        {
            bool result;
            bool checkEqual = false;

            var foundUser = await _userService.CheckByLogin(model.Login!);

            var equal = await _userService.FindUserByLogin(model.Login!);
            if (equal != null)
                checkEqual = equal.Login == model.Login;

            if (foundUser && checkEqual)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            if (result)
                return Json(false);
            return Json(true);
        }
    }
}
