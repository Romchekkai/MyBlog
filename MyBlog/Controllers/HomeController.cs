using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUserService userservice, IMapper mapper, IArticleService articleService)
        {
            _logger = logger;
            _userService = userservice;
            _mapper = mapper;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()// вхродящий параметр
        {
            var user = User;
            var posts = await _articleService.GetArticles();
            if (user.Identity.Name is not null && posts != null)
            {
                var postModels = _mapper.Map<IEnumerable<PostViewModel>>(posts);

                return View(postModels);
            }

            return View();
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
