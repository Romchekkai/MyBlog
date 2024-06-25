using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Api.Contracts.Services;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;


namespace MyBlog.Api.Controllers
{
    public class CommentManagerController : Controller
    {
        private ICommentService _commentService;

        public CommentManagerController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost]
        [Route("createComment")]
        public async Task<IActionResult> CreateComment(CommentCreateRequest model)
        {
            try
            {
                await _commentService.CreateComment(model);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("getArticleComments")]
        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid id)
        {
            try
            {
                var comments = await _commentService.GetCommentsByArticleId(id);
                return comments;
            }
            catch
            {
                throw new Exception("Произошла внутренняя ошибка (Get Article Comments)");
            }
        }
        [Authorize]
        [HttpGet]
        [Route("getComment")]
        public async Task<Comment> GetCommentById(Guid id)
        {
            try
            {
                var comment = await _commentService.GetCommentById(id); 
                return comment;
            }
            catch
            {
                throw new Exception("Произошла внутренняя ошибка (Get Comment)");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteComment")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentService.DeleteCommentById(id);
                return StatusCode(200);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return StatusCode(404); }
        }

        [Authorize]
        [HttpPatch]
        [Route("editComment")]
        public IActionResult EditComment(CommentEditRequest model)
        {
            try
            {
                _commentService.UpdateComment(model); 
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
    }
}
