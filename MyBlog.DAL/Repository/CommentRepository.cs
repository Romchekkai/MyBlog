using Microsoft.EntityFrameworkCore;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    public class CommentRepository: ICommentRepository
    {
        private readonly ApplicationContext _context;
        public CommentRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task CreateComment(CommentEntity comment)
        {
            var commentCheck = _context.Comments.Any(a => a.Id == comment.Id);

            if (!commentCheck)
            {
                await _context.Comments.AddAsync(comment);
                _context.SaveChanges();
            }
        }
       /* public async Task<IEnumerable<ArticleEntity>> GetAllArticles()
        {
            var articles = await _context.Articles.Include(p => p.Comments).Include(t => t.Tags).ToListAsync();
            if (articles != null)
                return articles;

            return null;
        }*/

        public async Task<IEnumerable<CommentEntity>> FindCommentsByArticleId(Guid id)
        {
            var comments = await _context.Comments.Where(i=>i.Id!=id).Include(u => u.User).ToListAsync();
            if (comments != null)
                return comments;

           

            return null;
        }

        public async Task<CommentEntity> FindCommentById(Guid id)
        {
            var comment = await _context.Comments.Include(u=>u.User).FirstOrDefaultAsync(a => a.Id == id);
            if (comment != null) return comment;
            return null;
        }

        public async Task DeleteCommentById(Guid id)
        {
            var commentoDelete = await FindCommentById(id);
            if (commentoDelete != null)
                _context.Comments.Remove(commentoDelete);
            _context.SaveChanges();
        }

        public async Task UpdateComment(CommentEntity comment)
        {
            var commentToUpdate = await FindCommentById(comment.Id);
            if (commentToUpdate != null)
                _context.Update(commentToUpdate);
            _context.SaveChanges();
        }

    }
    public interface ICommentRepository
    {
        Task CreateComment(CommentEntity comment);
        Task<IEnumerable<CommentEntity>> FindCommentsByArticleId(Guid id);
        Task<CommentEntity> FindCommentById(Guid id);
        Task DeleteCommentById(Guid id);
        Task UpdateComment(CommentEntity comment);
    }
}
