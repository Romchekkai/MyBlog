using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.AccountModel
{
    public class TagViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле тэг обязательно для заполнения")]
        public string Name { get; set; }
        public Guid ArticleId { get; set; }
    }
}
