using AutoMapper;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;
using MyBlog.Api.DLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.Contracts.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        public CommentService(IMapper mapper, ICommentRepository commentRepository)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
        }
        public async Task CreateComment(CommentCreateRequest model)
        {
            var comment = _mapper.Map<Comment>(model);
            await _commentRepository.CreateComment(comment);
        }
        public async Task DeleteCommentById(Guid id)
        {
            await _commentRepository.DeleteCommentById(id);
        }
        public void UpdateComment(CommentEditRequest model)
        {
            var entity = _mapper.Map<Comment>(model);
            _commentRepository.UpdateComment(entity);
        }
        public async Task<Comment> GetCommentById(Guid id)
        {
            var searchingComment = await _commentRepository.FindCommentById(id);
            return searchingComment;
        }
        public async Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid id)
        {
            var comments = await _commentRepository.FindCommentsByArticleId(id);
           // var commentModels = _mapper.Map<IEnumerable<CommentModel>>(comments);
            return comments;
        }       
    }
    public interface ICommentService
    {
        Task CreateComment(CommentCreateRequest model);
        Task DeleteCommentById(Guid id);
        void UpdateComment(CommentEditRequest model);
        Task<Comment> GetCommentById(Guid id);
        Task<IEnumerable<Comment>> GetCommentsByArticleId(Guid id);
    }
}
