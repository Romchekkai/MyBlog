using AutoMapper;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;
using MyBlog.Api.DLL.Repositories;
using System.Text;

namespace MyBlog.Api.Contracts.Services
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepo;
        private IMapper _mapper;

        public ArticleService(IArticleRepository articleRepo, IMapper mapper)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
        }
        public async Task CreateArticle(ArticleCreateRequest model)
        {
            var article = _mapper.Map<Article>(model);
            await _articleRepo.CreateArticle(article);
        }

        public async Task<Article> FindArticleById(Guid id)
        {
            var searchingArticle = await _articleRepo.FindArticleById(id);
            return searchingArticle;
        }
        public async Task<IEnumerable<Article>> GetArticles()
        {
            var articles = await _articleRepo.GetAllArticles();
            return articles;
        }
        public async Task<IEnumerable<Article>> GetUserArticles(Guid id)
        {
            var userArticles = await _articleRepo.FindArticlesByUserId(id);
            return userArticles;
        }

        public async Task DeleteArticle(Guid id)
        {
            await _articleRepo.DeleteArticleById(id);
        }
        public void UpdateArticle(ArticleEditRequest model)
        {
            var articleToUpdate = _mapper.Map<Article>(model);
            _articleRepo.UpdateArticle(articleToUpdate);
        }
        

        public async Task AddTag(TagCreateRequest model)
        {
            var tag = _mapper.Map<Tag>(model);
            if (model != null)
                await _articleRepo.AddTag(tag);
        }
        public async Task<Tag> FindTagById(int id)
        {
            var tagEntity = await _articleRepo.FindTagById(id);
            return tagEntity;
        }
        public async Task<IEnumerable<Tag>> GetTags()
        {
            var tags = await _articleRepo.GetTags();
            return tags;
        }
        public async Task DeleteTag(int id)
        {
            await _articleRepo.DeleteTagById(id);
        }
        public void UpdateTag(TagEditRequest model)
        {
            var tagEntity = _mapper.Map<Tag>(model);
            _articleRepo.UpdateTag(tagEntity);
        }
    }
    public interface IArticleService
    {
        Task CreateArticle(ArticleCreateRequest model);
        Task<Article> FindArticleById(Guid id);
        Task<IEnumerable<Article>> GetArticles();
        Task<IEnumerable<Article>> GetUserArticles(Guid id);
        Task DeleteArticle(Guid id);
        void UpdateArticle(ArticleEditRequest model);   
        Task AddTag(TagCreateRequest model);
        Task<Tag> FindTagById(int id);
        Task<IEnumerable<Tag>> GetTags();
        Task DeleteTag(int id);
        void UpdateTag(TagEditRequest model);
        
    }
}
