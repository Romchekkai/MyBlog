using Microsoft.EntityFrameworkCore;
using MyBlog.Api.DLL.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _context;
        public CommentRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task CreateComment(Comment comment)
        {
            var commentCheck = _context.Comments.Any(a => a.Id == comment.Id);

            if (!commentCheck)
            {
                await _context.Comments.AddAsync(comment);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Comment>> FindCommentsByArticleId(Guid id)
        {
            var comments = await _context.Comments.Where(i => i.Id != id).Include(u => u.User).ToListAsync();
            if (comments != null)
                return comments;



            return null;
        }

        public async Task<Comment> FindCommentById(Guid id)
        {
            var comment = await _context.Comments.Include(u => u.User).FirstOrDefaultAsync(a => a.Id == id);
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

        public void UpdateComment(Comment comment)
        {
            _context.Update(comment);
            _context.SaveChanges();
        }

    }
    public interface ICommentRepository
    {
        Task CreateComment(Comment comment);
        Task<IEnumerable<Comment>> FindCommentsByArticleId(Guid id);
        Task<Comment> FindCommentById(Guid id);
        Task DeleteCommentById(Guid id);
        public void UpdateComment(Comment comment);
    }
}
