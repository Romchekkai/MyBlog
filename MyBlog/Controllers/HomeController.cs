using Microsoft.AspNetCore.Mvc;
using MyBlog.BLL.Models;
using MyBlog.BLL.Services.UserServices;
using MyBlog.DAL.Entities;
using MyBlog.Models;
using System.Diagnostics;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userservice)
        {
            _logger = logger;
            _userService = userservice;
        }

        public IActionResult Index()
        {

            return View();
        }
        /* [HttpGet]
         public async Task Index()
         {
             string content = @"<form method='post'>
                 <label>Login:</label><br />
                 <input name='Login' /><br />
                 <label>Email:</label><br />
                 <input name='email' /><br />
                 <label>Дата рождения: </label><br />
                 <input type='date'name='DateOfTheBirth'/><br />
                 <label>About:</label><br />
                 <input name='About' /><br />
                 <label>Пароль: </label><br />
                 <input type='password'name='Password'/><br />
                 <input type='submit' value='Send' />
             </form>";
             Response.ContentType = "text/html;charset=utf-8";
             await Response.WriteAsync(content);
         }*/
        [HttpPost]
        public async Task<IActionResult> Index(UserModel model)
        {

            /*Using Request form
            #pragma warning disable CS8601 // Possible null reference assignment.
            model.Login = Request.Form["Login"];
            #pragma warning restore CS8601 // Possible null reference assignment.
            #pragma warning disable CS8601 // Possible null reference assignment.
            model.Password = Request.Form["Password"];
            #pragma warning restore CS8601 // Possible null reference assignment.
            #pragma warning disable CS8601 // Possible null reference assignment.
            model.Email = Request.Form["Email"];
            #pragma warning restore CS8601 // Possible null reference assignment.*/


            /* model = new UserModel()
             {
                 Id = 1,
                 FirstName = "ee",
                 Surname = "qwer",
                 Login = "log",
                 Email = "email@gmail.com",
                 Password = "123",
                 ModelRole = new UserRoleModel() { Id = 1, Name = "Admin" },
                 DateOfTheBirth = DateTime.Now
             };*/

            await _userService.CreateUser(model,model.Password);

            return View("Index",model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
