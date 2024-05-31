using MyBlog.DAL.Entities;

namespace MyBlog.Models.AccountModel
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public DateTime DatePosted { get; set; }
        public List<CommentViewModel> Comments { get; set; }

    }
}
