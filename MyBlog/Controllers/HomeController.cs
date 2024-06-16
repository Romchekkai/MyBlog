using AutoMapper;
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
            IArticleService articleService, ICommentService commentService)
        {
            _logger = logger;
            _userService = userservice;
            _mapper = mapper;
            _articleService = articleService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()// вхродящий параметр
        {
            var user = User;
            var posts = await _articleService.GetArticles();
            var postModels = _mapper.Map<IEnumerable<PostViewModel>>(posts);
            return View(postModels);
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
            var usersModel = await _userService.GetAllUsers();
            var usersView = _mapper.Map<IEnumerable<MainViewModel>>(usersModel);

            return View(usersView);
        }

        public async Task<IActionResult> CreateComment(CommentViewModel model)
        {
            var form = HttpContext.Request.Form;
            var ArticleID = form["modelID"];
            var UserID = form["userID"];

            Guid guidArticle = new Guid();
            Guid guidUser = new Guid();

            if (!ArticleID.IsNullOrEmpty())
                guidArticle = new Guid($"{ArticleID}");
            if (!UserID.IsNullOrEmpty())
                guidUser = new Guid($"{UserID}");

            var commentModel = _mapper.Map<CommentModel>(model);
            commentModel.ArticleId = guidArticle;
            commentModel.UserId = guidUser;
            commentModel.CreatedDate = DateTime.Now;

            await _commentService.CreateComment(commentModel);

            return RedirectToAction("Index");
        }

        public IActionResult AuthenticationError()
        { return View(); }
        public IActionResult AuthorizationError()
        { return View(); }

        public IActionResult UnexpectedError()
        { return View(); }
        



    }
}
