using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBlog.BLL.Models.UserModels;
using MyBlog.BLL.Services.UserServices;
using MyBlog.DAL.Entities;
using MyBlog.Models.AccountModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyBlog.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;
        private IMapper _mapper;
        private IArticleService _articleService;

        public AccountManagerController(ILogger<HomeController> logger, IUserService userService, 
            IMapper mapper, IArticleService articleService)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _articleService = articleService;
        }

    

        // GET: UserController
        public ActionResult Login()
        {
            return View("Sign");
        }

       [HttpPost]
        public async Task<IActionResult> Login(SignView model)
        {
            var userModel = _mapper.Map<UserModel>(model.LoginViewModel);

            string login = userModel.Login;
            string password = userModel.Password;

            var user = await _userService.FindUserByLogin(login);
            if (user is null)
                return Redirect("/Home/UnexpectedError");
            if(user.Login != login || user.Password != password)
                return Redirect("/Home/UnexpectedError");

            //аутентификация с помощью Cookies
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), 
                new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name )}; 

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> UserPage()
        {
            var user = User;
            try
            {
                if (User.Identity!.IsAuthenticated)
                {
                    var userModel = await _userService.FindUserByLogin(user.Identity!.Name!);
                    var mainModel = _mapper.Map<MainViewModel>(userModel);

                    var articleModels = await _articleService.GetUserArticles(mainModel.Id);
                    var userArticles = _mapper.Map<IEnumerable<PostViewModel>>(articleModels);

                    mainModel.Posts = userArticles;

                    return View("UserPage", mainModel);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            

            return RedirectToAction("Login", "AccountManager"); 
        }

        public async Task<IActionResult> UserEdit()
        {
            var user = User;

            var usermodel = await _userService.FindUserByLogin(user.Identity!.Name!);

            var editModel = _mapper.Map<UserEditView>(usermodel);

            return View("UserEdit", editModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditView userEditView)
        {
            var userEditModel = _mapper.Map<UserModel>(userEditView);
            var user = await _userService.UpdateUser(userEditModel);

            if (HttpContext.User.Identity is ClaimsIdentity claimsIdentity)
            {
                if(userEditView.Login != user.Login)
                {
                    var userLogin = claimsIdentity.FindFirst(ClaimTypes.Name);
                    if (claimsIdentity.TryRemoveClaim(userLogin))
                    {
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                    }
                }
                if (userEditView.Email != user.Email)
                {
                    var userEmail = claimsIdentity.FindFirst(ClaimTypes.Email);
                    if (claimsIdentity.TryRemoveClaim(userEmail))
                    {
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                    }
                }
            }
           

            return RedirectToAction("UserPage", "AccountManager");
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(UserEditView model)
        {
            bool result = false;
            bool checkEqual = false;
            var foundUser = await _userService.CheckByEmail(model.Email);

            var equal = await _userService.FindUserByEmail(model.Email);
            if(equal != null)
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
        public async Task<IActionResult> VerifyLogin(UserEditView model)
        {
            bool result = false;
            bool checkEqual = false;

            var foundUser = await _userService.CheckByLogin(model.Login);

            var equal = await _userService.FindUserByLogin(model.Login);
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

       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserRole(Guid id)
        {
            var user = await _userService.FindUserById(id);
            var userRoleName = user.Role.Name;
            var username = user.GetFullName();

            // var viewmodel = new RoleEditView(id,userRoleName, username);
            var viewmodel = new RoleEditView() { UserId = id, RoleName = userRoleName, UserName = username };
            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditUserRole(RoleEditView view)
        { 

            await _userService.ChangeRole(view.Id, view.UserId);

            return RedirectToAction("EditUserRole", "AccountManager",view.UserId);
        }

    }
}
