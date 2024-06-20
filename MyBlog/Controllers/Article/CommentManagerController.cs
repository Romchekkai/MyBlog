using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.BLL.Models.UserModels;
using MyBlog.BLL.Services.UserServices;
using MyBlog.Controllers.Account;
using MyBlog.Models.AccountModel;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace MyBlog.Controllers.Article
{
    public class CommentManagerController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private ICommentService _commentService;
        private readonly ILogger<CommentManagerController> _logger;

        public CommentManagerController(IUserService userService, IMapper mapper, 
            ICommentService commentService, ILogger<CommentManagerController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _commentService = commentService;
            _logger = logger;
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

            try
            {
                await _commentService.CreateComment(commentModel);
            }
            catch
            {
                _logger.LogError($"Bad request to {HttpContext.Request.Path} Time:{DateTime.UtcNow.ToLongTimeString()}");          
            }
           
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

        public async Task<IActionResult> EditComment(Guid id)
        {
            var commentModel = await _commentService.GetCommentById(id);
            var commentView = _mapper.Map<CommentViewModel>(commentModel);

            if (commentView != null) return View(commentView);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public  IActionResult EditComment(CommentViewModel viewModel)
        {
            var model = _mapper.Map<CommentModel>(viewModel);

            _commentService.UpdateComment(model);

            return Redirect($"~/ArticleManager/OpenArticle/{model.ArticleId}");
        }


        [NonAction]
        public async Task<string> GetUserName(Guid id)
        {
           var user = await _userService.FindUserById(id);
            var usrname = user.Surname + " " + user.Surname;
            return usrname;
        }
    }
}
