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
            var article = await _context.Articles.Include(p => p.Comments).Include(t => t.Tags).FirstOrDefaultAsync(a => a.Id == id);
            if (article != null) return article;
            return null;
        }

        public async Task DeleteArticleById(Guid id)
        {
            var articletoDelete = await FindArticleById(id);
            if (articletoDelete != null)
                _context.Articles.Remove(articletoDelete);
            await _context.SaveChangesAsync();
        }

        public void UpdateArticle(ArticleEntity article)
        {
            
            if (article != null)
                _context.Update(article);
            _context.SaveChanges();
        }

        public async Task AddTags(List<TagEntity> tags)
        {
            if (tags != null)
                foreach(TagEntity tag  in tags)
                {
                    await _context.AddAsync(tag);
                    _context.SaveChanges();
                }
        }
        public async Task AddTag(TagEntity tag)
        {
            if (tag != null)
                    await _context.AddAsync(tag);
                    _context.SaveChanges();
        }

        public async Task<TagEntity> FindTagById(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(a => a.Id == id);
            if (tag != null) return tag;
            return null;
        }

        public void UpdateTag(TagEntity tag)
        {
            if (tag != null)
                _context.Update(tag);
            _context.SaveChanges();
        }

        public async Task DeleteTagById(int id)
        {
            var tagtoDelete = await FindTagById(id);
            if (tagtoDelete != null)
                _context.Tags.Remove(tagtoDelete);
            await _context.SaveChangesAsync();
        }

    }

    public interface IArticleRepository
    {
        Task CreateArticle(ArticleEntity article);
        Task<IEnumerable<ArticleEntity>> GetAllArticles();
        Task<IEnumerable<ArticleEntity>> FindArticlesByUserId(Guid id);
        Task<ArticleEntity> FindArticleById(Guid id);
        Task DeleteArticleById(Guid id);
        void UpdateArticle(ArticleEntity article);
        Task AddTags(List<TagEntity> tags);
        Task<TagEntity> FindTagById(int id);
        void UpdateTag(TagEntity tag);
        Task DeleteTagById(int id);
        Task AddTag(TagEntity tag);
    }
}
