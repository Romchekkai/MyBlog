using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using MyBlog.BLL.Models.UserModels;
using MyBlog.BLL.Services.UserServices;
using MyBlog.Models.AccountModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace MyBlog.Controllers.Article
{
    public class ArticleManagerController : Controller
    {
        StringValues strings = new StringValues();
        private IUserService _userService;
        private IMapper _mapper;
        private IArticleService _articleService;

        public ArticleManagerController(IUserService userService, IMapper mapper, IArticleService articleService)
        {
            _userService = userService;
            _mapper = mapper;
            _articleService = articleService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewArticle(CreateArticleView model)
        {
            var articleModel = await GetArticlesWithTags(model);

            if (articleModel != null)
            {
                await _articleService.CreateArticle(articleModel);
            }

            return Redirect("~/AccountManager/UserPage");
        }

        public async Task<IActionResult> EditArticle(Guid id)
        {
                var article = await _articleService.FindArticleById(id);
                var articleView = _mapper.Map<PostViewModel>(article);

                if (article != null) return View(articleView);
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditArticle(PostViewModel model)
        {
            var articleUpdated = _mapper.Map<ArticleModel>(model);

            _articleService.UpdateArticle(articleUpdated);

            return Redirect($"~/ArticleManager/EditArticle/{model.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            await _articleService.DeleteArticle(id);

            return RedirectToAction("UserPage", "AccountManager");
        }

        public async Task<IActionResult> OpenArticle(Guid id)
        {
            var article = await _articleService.FindArticleById(id);
            var articleView = _mapper.Map<PostViewModel>(article);

            if (article != null) return View(articleView);
            return NotFound();
        }

        [HttpPost]
        public IActionResult OpenArticle(PostViewModel viewmodel)
        {
            var model = viewmodel;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(int id)
        {
            var tag = await _articleService.FindTagById(id);

            var tagView = _mapper.Map<TagViewModel>(tag);

            return View(tagView);
        }
        [HttpPost]
        public IActionResult EditTag(TagViewModel model)
        {
            strings = Request.Headers.Referer;
            var tagModel = _mapper.Map<TagModel>(model);

            _articleService.UpdateTag(tagModel);

            
            return Redirect($"~/ArticleManager/EditArticle/{model.ArticleId}");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _articleService.FindTagById(id);
            var idRoute = tag.ArticleId.ToString();

            await _articleService.DeleteTag(id);

            return Redirect($"~/ArticleManager/EditArticle/{idRoute}");
        }

        public async Task<IActionResult> AddTag(TagViewModel modelTag)
        {
            var form = HttpContext.Request.Form;
            var ArticleID = form["modelID"];

           

            var guidArticle = new Guid($"{ArticleID}");

            var tagModel = _mapper.Map<TagModel>(modelTag);
            tagModel.ArticleId = guidArticle;

            await _articleService.AddTag(tagModel);

            return Redirect($"~/ArticleManager/EditArticle/{guidArticle}");
        }


        

        [NonAction]
        public async Task<ArticleModel> GetArticlesWithTags(CreateArticleView model )
        {

            var form = HttpContext.Request.Form;

            var tags = new List<string>() { form["tag1"], form["tag2"], form["tag3"] };

            var articleModel = _mapper.Map<ArticleModel>(model);

            var currentUser = await _userService.FindUserByLogin(User.Identity.Name);
            articleModel.UserId = currentUser.Id;
            articleModel.DatePosted = DateTime.Now;

            foreach (var tag in tags)
            {
                if(tag !=null)
                articleModel.Tags.Add(new TagModel() { Name = tag });
            }


            return articleModel;
        }

    }
}
