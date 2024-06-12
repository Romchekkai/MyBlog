using MyBlog.DAL.Entities;

namespace MyBlog.Models.AccountModel
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public string Tag { get; set; }

        public TagViewModel modelTag { get; set; }

        public CommentViewModel commentViewModel { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; } 
        public DateTime DatePosted { get; set; }
        public List<CommentViewModel> Comments { get; set; }

    }
}
