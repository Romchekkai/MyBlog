using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BLL.Services.UserServices;
using MyBlog.DAL.Entities;
using MyBlog.Models.AccountModel;
using System.Security.Claims;
using MyBlog.BLL.Models.UserModels;

namespace MyBlog.Controllers.Account
{
    public class RegisterController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;
        private IMapper _mapper;

        public RegisterController(ILogger<HomeController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }


        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(SignView model)
        {
            RegisterViewModel modelInfo = model.RegisterViewModel;

            var foundUser = await _userService.CheckByEmail(modelInfo.Email);

            if (foundUser)
                return Json(false);
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyLogin(SignView model)
        {
            RegisterViewModel modelInfo = model.RegisterViewModel;

            var foundUser = await _userService.CheckByLogin(modelInfo.Login);

            if (foundUser)
                return Json(false);
            return Json(true);
        }
        public async Task<IActionResult> VerifyForLogin(SignView model)
        {
            LoginViewModel modelInfo = model.LoginViewModel;

            var foundUser = await _userService.CheckByLogin(modelInfo.Login);
            if (!foundUser)
                return Json(false);
            return Json(true);
        }
        /*public async Task<IActionResult> VerifyPassword(SignView model)
        {
            LoginViewModel modelInfo = model.LoginViewModel;

            var loginprev = LoginUserCheck;

            var foundUser = await _userService.CheckByPassword(modelInfo.Password, loginprev);

            if (!foundUser)
                return Json(false);
            return Json(true);
        }*/

        [HttpPost]
        public async Task<IActionResult> Register(SignView model)
        {

            var userModel = _mapper.Map<UserModel>(model.RegisterViewModel);
            if(userModel != null)
            {
                await _userService.CreateUser(userModel);

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userModel.Login), new Claim(ClaimTypes.Email, userModel.Email)};

                ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
