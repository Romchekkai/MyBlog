using Microsoft.EntityFrameworkCore;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    public class ArticleRepository:IArticleRepository
    {
        private readonly ApplicationContext _context;
        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task CreateArticle(ArticleEntity article)
        {
            var articleCheck = _context.Articles.Any(a=>a.Id == article.Id);

            if (!articleCheck) {
                await _context.Articles.AddAsync(article);
                _context.SaveChanges();
            }
        }
        public async Task<IEnumerable<ArticleEntity>> GetAllArticles()
        {
            var articles = await _context.Articles.Include(p => p.Comments).Include(t=>t.Tags).ToListAsync();
            if(articles != null)
                return articles;

            return null;
        }

        public async Task<IEnumerable<ArticleEntity>> FindArticlesByUserId(Guid id)
        {
            var userArticles = await _context.Articles.Include(p => p.Comments).Include(t => t.Tags).
                Where(a => a.UserId == id).ToListAsync();
            if (userArticles != null)
                return userArticles;

            return null;
        }

        public async Task<ArticleEntity> FindArticleById(Guid id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article != null) return article;
            return null;
        }

        public async Task DeleteArticleById(Guid id)
        {
            var articletoDelete = await FindArticleById(id);
            if (articletoDelete != null)
                _context.Articles.Remove(articletoDelete);
        }

        public async Task UpdateArticle(ArticleEntity article)
        {
            var articleToUpdate = await FindArticleById(article.Id);
            if (articleToUpdate != null)
                _context.Update(articleToUpdate);
            _context.SaveChanges();
        }

    }

    public interface IArticleRepository
    {
        Task CreateArticle(ArticleEntity article);
        Task<IEnumerable<ArticleEntity>> GetAllArticles();
        Task<IEnumerable<ArticleEntity>> FindArticlesByUserId(Guid id);
        Task<ArticleEntity> FindArticleById(Guid id);
        Task DeleteArticleById(Guid id);
        Task UpdateArticle(ArticleEntity article);
    }
}
