using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.BLL.Models.UserModels;
using MyBlog.BLL.Services.UserServices;
using MyBlog.Models.AccountModel;
using System.ComponentModel.Design;

namespace MyBlog.Controllers.Article
{
    public class CommentManagerController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private IArticleService _articleService;
        private ICommentService _commentService;

        public CommentManagerController(IUserService userService, IMapper mapper, IArticleService articleService, ICommentService commentService)
        {
            _userService = userService;
            _mapper = mapper;
            _articleService = articleService;
            _commentService = commentService;
        }
        
       
        public async Task<IActionResult> CreateComment(CommentViewModel commentViewModel)
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

            var commentModel = _mapper.Map<CommentModel>(commentViewModel);
            commentModel.ArticleId = guidArticle;
            commentModel.UserId = guidUser;
            commentModel.CreatedDate = DateTime.Now;


            await _commentService.CreateComment(commentModel);

            return Redirect($"~/ArticleManager/OpenArticle/{guidArticle}");
        }

        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _commentService.GetCommentById(id);
            var idRoute = comment.ArticleId.ToString();
            try
            {            
                await _commentService.DeleteCommentById(id);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return Redirect($"~/ArticleManager/OpenArticle/{idRoute}");
        }
    }
}
