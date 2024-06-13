using Microsoft.Extensions.Hosting;
using MyBlog.DAL.Entities;

namespace MyBlog.Models.AccountModel
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Author {  get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
