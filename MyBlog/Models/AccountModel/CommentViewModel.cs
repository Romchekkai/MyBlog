using Microsoft.Extensions.Hosting;

namespace MyBlog.Models.AccountModel
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid AuthorID { get; set; }
        public string Author {  get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }   
    }
}
