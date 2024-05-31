using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                return (IActionResult)Results.Unauthorized();
            if(user.Login != login || user.Password != password)
                return (IActionResult)Results.Unauthorized();

            //аутентификация с помощью Cookies
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.Email, user.Email) }; 

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UserPage()
        {
            var user = User;
            if (User.Identity.IsAuthenticated)
            {
                var userModel = await _userService.FindUserByLogin(user.Identity.Name);
                var mainModel = _mapper.Map<MainViewModel>(userModel);

                var articleModels = await _articleService.GetUserArticles(mainModel.Id);
                var userArticles = _mapper.Map<IEnumerable<PostViewModel>>(articleModels);

                mainModel.Posts = userArticles;

                return View("UserPage", mainModel);
            }

            return RedirectToAction("Login", "AccountManager"); 
        }

        public async Task<IActionResult> UserEdit()
        {
            var user = User;

            var usermodel = await _userService.FindUserByLogin(user.Identity?.Name);

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
            var mainModel = _mapper.Map<MainViewModel>(user);

            return View("UserPage", mainModel);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(UserEditView model)
        {
            var foundUser = await _userService.CheckByEmail(model.Email);

            if (foundUser)
                return Json(false);
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyLogin(UserEditView model)
        {

            var foundUser = await _userService.CheckByLogin(model.Login);

            if (foundUser)
                return Json(false);
            return Json(true);
        }

      /*  [HttpGet]
        public async Task<IActionResult> NewArticle()
        {

        }*/

        [HttpPost]
        public async Task<IActionResult> NewArticle()
        {
            var user = User;

            var usermodel = await _userService.FindUserByLogin(user.Identity?.Name);

            ArticleModel model = new ArticleModel()
            {
                Title = "ads",
                Description = "aaa",
                UserId = usermodel.Id
            };
            await _articleService.CreateArticle(model);
            return RedirectToAction("UserPage", "AccountManager");
        }

        [HttpPost]

        // GET: UserController/Details/5
        [Authorize]
        public ActionResult Details()
        {
            return Content("Hello World!");
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
