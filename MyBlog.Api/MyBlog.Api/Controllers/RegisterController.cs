using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyBlog.Api.Contracts.Services;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;

namespace MyBlog.Api.Controllers
{
    /// <summary>
    /// Register users
    /// </summary>
    public class RegisterController : Controller
    {
        private IUserService _userService;
        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="model"> Register data request</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            try
            { 
                if (model != null)
                {
                    await _userService.CreateUser(model);

                    var claims = new List<Claim> { new Claim(ClaimTypes.Name,
                        model.Login), new Claim(ClaimTypes.Email, model.Email)
                        ,new Claim(ClaimsIdentity.DefaultRoleClaimType,"User")};


                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");

                    var timeAuth = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    await HttpContext.SignInAsync
                        (CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity), timeAuth);
                }
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(404);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(RegisterRequest model)
        {
            var foundUser = await _userService.CheckByEmail(model.Email!);

            if (foundUser)
                return Json(false);
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyLogin(RegisterRequest model)
        {
            var foundUser = await _userService.CheckByLogin(model.Login);

            if (foundUser)
                return Json(false);
            return Json(true);
        }
        public async Task<IActionResult> VerifyForLogin(RegisterRequest model)
        {
            var foundUser = await _userService.CheckByLogin(model.Login);
            if (!foundUser)
                return Json(false);
            return Json(true);
        }
    }
}
