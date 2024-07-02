using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.BLL.Models;
using MyBlog.BLL.Models.UserModels;
using MyBlog.BLL.Services.UserServices;
using MyBlog.DAL.Entities;
using MyBlog.Models;
using MyBlog.Models.AccountModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;
        private IArticleService _articleService;
        private ICommentService _commentService;
        private IMapper _mapper;
        

        public HomeController(ILogger<HomeController> logger, 
            IUserService userservice, IMapper mapper, 
            IArticleService articleService, 
            ICommentService commentService)
        {
            _logger = logger;
            _userService = userservice;
            _mapper = mapper;
            _articleService = articleService;
            _commentService = commentService;

        }

        public async Task<IActionResult> Index()// вхродящий параметр
        {
            try
            {
                var posts = await _articleService.GetArticles();
                var postModels = _mapper.Map<IEnumerable<PostViewModel>>(posts);
                return View(postModels);
            }
            catch
            {
                _logger.LogError($"Bad request to {HttpContext.Request.Path} Time:{DateTime.UtcNow.ToLongTimeString()}");
                return RedirectToAction("UnexpectedError", "Home"); 
            }
            
           
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

        public async Task<IActionResult> UsersPage()
        {
            try
            {
                var usersModel = await _userService.GetAllUsers();
                var usersView = _mapper.Map<IEnumerable<MainViewModel>>(usersModel);
                return View(usersView);
            }
            catch
            {
                _logger.LogError($"Bad request to {HttpContext.Request.Path} Time:{DateTime.UtcNow.ToLongTimeString()}");
                return RedirectToAction("UnexpectedError", "Home");
            }
            
        }
        [Authorize]
        public async Task<IActionResult> CreateComment(CommentViewModel model)
        {
            try
            {
                var user = await _userService.FindUserByLogin(User.Identity!.Name!);
                var userId = user.Id;

                var form = HttpContext.Request.Form;
                var ArticleID = form["modelID"];
                Guid guidArticle = new Guid();
                if (!ArticleID.IsNullOrEmpty())
                    guidArticle = new Guid($"{ArticleID}");


                var commentModel = new CommentModel();
                commentModel.ArticleId = guidArticle;
                commentModel.UserId = userId;
                commentModel.Text= model.Text;
                commentModel.CreatedDate = DateTime.UtcNow;

                await _commentService.CreateComment(commentModel);

               // _logger.LogInformation("Comment has created ");

                return RedirectToAction("Index");
            }
            catch
            {
                _logger.LogError($"Bad request to {HttpContext.Request.Path} Time:{DateTime.UtcNow.ToLongTimeString()}");
                return RedirectToAction("UnexpectedError", "Home");
            }
            
        }

        public IActionResult AuthenticationError()
        { return View(); }
        public IActionResult AuthorizationError()
        { return View(); }

        [Route("/Home/UnexpectedError")]
        public IActionResult UnexpectedError()
        {
            _logger.LogError("Произошла непредвиденная ошибка");
            return View(); 
        }     
    }
}
