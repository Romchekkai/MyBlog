using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Api.Contracts.Services;
using MyBlog.Api.DLL.Models.RequestModels;
using System.Security.Claims;
using MyBlog.Api.DLL.Models.ResponseModels;

namespace MyBlog.Api.Controllers
{
   /// <summary>
   /// Manage user data and authenticate
   /// </summary>
    public class AccountManagerController : Controller
    {
        private IUserService _userService;

        public AccountManagerController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="request"> login request</param>
        /// <returns>cookie</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
                throw new ArgumentNullException("Ошибка ввода значения");

            var user = await _userService.FindUserByLogin(request.Login);
            if (user.Password != request.Password)
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
        /// <summary>
        /// Completed cookie's session
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCode(200); 
        }
        /// <summary>
        /// Create user, admin role only
        /// </summary>
        /// <param name="model"></param>
        /// <returns> 200ok</returns>
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
        /// <summary>
        /// get user data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// get users data
        /// </summary>
        /// <returns>User's list</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// delte user
        /// </summary>
        /// <param name="id"> Guid </param>
        /// <returns></returns>
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
        /// <summary>
        /// edit user
        /// </summary>
        /// <param name="model"> edited user data</param>
        /// <returns></returns>
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
