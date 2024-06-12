namespace MyBlog.Models.AccountModel
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ArticleId { get; set; }
    }
}
