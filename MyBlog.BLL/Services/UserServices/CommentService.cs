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
    public class CommentService: ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        public CommentService(IMapper mapper, ICommentRepository commentRepository)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
        }
        public async Task CreateComment(CommentModel model)
        {
            var comment = _mapper.Map<CommentEntity>(model);
            await _commentRepository.CreateComment(comment);
        }
        public async Task<IEnumerable<CommentModel>> GetCommentsByArticleId(Guid id)
        {
            var comments = await _commentRepository.FindCommentsByArticleId(id);
            var commentModels = _mapper.Map<IEnumerable<CommentModel>>(comments);
            return commentModels;
        }

        public async Task DeleteCommentById(Guid id)
        {
            await _commentRepository.DeleteCommentById(id);
        }

        public async Task<CommentModel> GetCommentById(Guid id)
        {
            try
            {
                var searchingComment = await _commentRepository.FindCommentById(id);
                var commentModel = _mapper.Map<CommentModel>(searchingComment);
                return commentModel;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return null;
        }
    }
    public interface ICommentService
    {
        Task CreateComment(CommentModel model);
        Task<IEnumerable<CommentModel>> GetCommentsByArticleId(Guid id);
        Task DeleteCommentById(Guid id);
        Task<CommentModel> GetCommentById(Guid id);

    }
}
