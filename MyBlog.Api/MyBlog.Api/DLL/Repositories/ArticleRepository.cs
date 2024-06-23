using Microsoft.EntityFrameworkCore;
using MyBlog.Api.DLL.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationContext _context;
        public ArticleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task CreateArticle(Article article)
        {
            var articleCheck = _context.Articles.Any(a => a.Id == article.Id);

            if (!articleCheck)
            {
                await _context.Articles.AddAsync(article);
                _context.SaveChanges();
            }
        }
        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            var articles = await _context.Articles.Include(p => p.Comments).ThenInclude(u => u.User).Include(a => a.User).ThenInclude(r => r.Role).Include(t => t.Tags).ToListAsync();
            if (articles != null)
                return articles;

            return null;
        }

        public async Task<IEnumerable<Article>> FindArticlesByUserId(Guid id)
        {
            var userArticles = await _context.Articles.Include(p => p.Comments).ThenInclude(u => u.User).ThenInclude(r => r.Role).Include(t => t.Tags).
                Where(a => a.UserId == id).ToListAsync();
            if (userArticles != null)
                return userArticles;

            return null;
        }

        public async Task<Article> FindArticleById(Guid id)
        {
            var article = await _context.Articles.Include(a => a.User).ThenInclude(r => r.Role).Include(p => p.Comments).ThenInclude(u => u.User).Include(t => t.Tags).FirstOrDefaultAsync(a => a.Id == id);
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

        public void UpdateArticle(Article article)
        {

            if (article != null)
                _context.Update(article);
            _context.SaveChanges();
        }

        public async Task AddTags(List<Tag> tags)
        {
            if (tags != null)
                foreach (Tag tag in tags)
                {
                    await _context.AddAsync(tag);
                    _context.SaveChanges();
                }
        }
        public async Task AddTag(Tag tag)
        {
            if (tag != null)
                await _context.AddAsync(tag);
            _context.SaveChanges();
        }

        public async Task<Tag> FindTagById(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(a => a.Id == id);
            if (tag != null) return tag;
            return null;
        }

        public void UpdateTag(Tag tag)
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
        Task CreateArticle(Article article);
        Task<IEnumerable<Article>> GetAllArticles();
        Task<IEnumerable<Article>> FindArticlesByUserId(Guid id);
        Task<Article> FindArticleById(Guid id);
        Task DeleteArticleById(Guid id);
        void UpdateArticle(Article article);
        Task AddTags(List<Tag> tags);
        Task<Tag> FindTagById(int id);
        void UpdateTag(Tag tag);
        Task DeleteTagById(int id);
        Task AddTag(Tag tag);
    }
}
