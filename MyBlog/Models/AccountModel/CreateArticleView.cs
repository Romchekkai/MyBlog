using MyBlog.DAL.Entities;

namespace MyBlog.Models.AccountModel
{
    public class CreateArticleView
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
