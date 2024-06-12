using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.BLL.Extensions;
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
           

           var tags = new List<TagEntity>();
            tags.Convert(model.Tags);

           

            article.Tags = tags;    

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

        public void UpdateArticle(ArticleModel model)
        {
            var articleToUpdate = _mapper.Map<ArticleEntity>(model);
             _articleRepo.UpdateArticle(articleToUpdate);
        }

        public async Task<TagModel> FindTagById(int id)
        {
            var tagEntity = await _articleRepo.FindTagById(id);
            var tagModel = _mapper.Map<TagModel>(tagEntity);

            return tagModel;
        }
        public void UpdateTag(TagModel model)
        {
            var tagEntity = _mapper.Map<TagEntity>(model);
            _articleRepo.UpdateTag(tagEntity);
        }
        public async Task DeleteTag(int id)
        {
            await _articleRepo.DeleteTagById(id);
        }
        public async Task AddTag(TagModel model)
        {
            var tagEntity = _mapper.Map<TagEntity>(model);
            if (model != null)
                await _articleRepo.AddTag(tagEntity);        
        }

    }
    public interface IArticleService
    {
        Task CreateArticle(ArticleModel model);
        Task<IEnumerable<ArticleModel>> GetArticles();
        Task<IEnumerable<ArticleModel>> GetUserArticles(Guid id);
        Task DeleteArticle(Guid id);
        Task<ArticleModel> FindArticleById(Guid id);
        void UpdateArticle(ArticleModel model);
        Task<TagModel> FindTagById(int id);
        void UpdateTag(TagModel model);
        Task DeleteTag(int id);
        Task AddTag(TagModel model);
    }
}
