using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Api.Contracts.Services;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;

namespace MyBlog.Api.Controllers
{
    public class ArticleManagerController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private IArticleService _articleService;


        public ArticleManagerController(IUserService userService, IMapper mapper,
            IArticleService articleService)
        {
            _userService = userService;
            _mapper = mapper;
            _articleService = articleService;
        }

        [Authorize]
        [HttpPost]
        [Route("createArticle")]
        public async Task<IActionResult> NewArticle(ArticleCreateRequest model)
        {
            try
            {
                await _articleService.CreateArticle(model);
                return StatusCode(200);
            }
            catch
            {            
                return StatusCode(400);
            }
        }
        [Route("article")]
        [HttpGet]
        public async Task<Article> GetArticle(Guid id)
        {
            try
            {
                var article = await _articleService.FindArticleById(id);
                return article;
            }
            catch
            {
                throw new ArgumentNullException(nameof(id));
            }
        }
        [Route("articles")]
        [HttpGet]
        public async Task<IEnumerable<Article>> GetArticles()
        {
            try
            {
                var articles = await _articleService.GetArticles();
                return articles;
            }
            catch
            {
                throw new Exception("Неизвестная ошибка при открытии статей");
            }
        }
        [HttpGet]
        [Route("userArticles")]
        public async Task<IEnumerable<Article>> GetUserArticles(Guid id)
        {
            try
            {
                var articles = await _articleService.GetUserArticles(id);
                return articles;
            }
            catch
            {
                throw new Exception("Неизвестная ошибка при открытии статей пользователя");
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("deleteArticle")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            try
            {
                await _articleService.DeleteArticle(id);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(404);
            }
            
        }
        [Authorize]
        [HttpPatch]
        [Route("editArticle")]
        public IActionResult EditArticle(ArticleEditRequest model)
        {
            try
            {
                _articleService.UpdateArticle(model);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("addTag")]
        public async Task<IActionResult> AddTag(TagCreateRequest request)
        {
            try
            {
                await _articleService.AddTag(request);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
        [HttpGet]
        [Route("tags")]
        public async Task<IEnumerable<Tag>> GetTags()
        {
            try
            {
                var tags = await _articleService.GetTags();
                return tags;
            }
            catch
            {
                throw new Exception("Неизвестная ошибка при поиске тэга");
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("deleteTag")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _articleService.FindTagById(id);
            var idRoute = tag.ArticleId.ToString();

            await _articleService.DeleteTag(id);

            return Redirect($"~/ArticleManager/EditArticle/{idRoute}");
        }

        [Authorize]
        [HttpPatch]
        [Route("editTag")]
        public IActionResult EditTag(TagEditRequest model)
        {
            try
            {
                _articleService.UpdateTag(model);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
        
    }
}
