using AutoMapper;
using MyBlog.BLL.Models.UserModels;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Services.UserServices
{
    public class ArticleService: IArticleService
    {
        private IArticleRepository _articleRepo;
        private IMapper _mapper;

        public ArticleService(IArticleRepository articleRepo, IMapper mapper)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
        }
        public async Task CreateArticle(ArticleModel model)
        {
            var article = _mapper.Map<ArticleEntity>(model);
            await _articleRepo.CreateArticle(article);
        }

        public async Task<IEnumerable<ArticleModel>> GetArticles()
        {
            var articles = await _articleRepo.GetAllArticles();
            var aricleArray = _mapper.Map<IEnumerable<ArticleModel>>(articles);

            return aricleArray;
        }
        public async Task<IEnumerable<ArticleModel>> GetUserArticles(Guid id)
        {
            var userArticles = await _articleRepo.FindArticlesByUserId(id);
            var Articles = _mapper.Map<IEnumerable<ArticleModel>>(userArticles);
            return Articles;
        }

        public async Task DeleteArticle(Guid id)
        {
           await _articleRepo.DeleteArticleById(id);
        }

        public async Task<ArticleModel> FindArticleById(Guid id)
        {
            try
            {
                var searchingArticle = await _articleRepo.FindArticleById(id);
                var articleModel = _mapper.Map<ArticleModel>(searchingArticle);
                return articleModel;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return null;
        }
    }
    public interface IArticleService
    {
        Task CreateArticle(ArticleModel model);
        Task<IEnumerable<ArticleModel>> GetArticles();
        Task<IEnumerable<ArticleModel>> GetUserArticles(Guid id);
        Task DeleteArticle(Guid id);
        Task<ArticleModel> FindArticleById(Guid id);
    }
}
