using MyBlog.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.AccountModel
{
    public class CreateArticleView
    {
        [Required(ErrorMessage = "Поле заголовок обязательно для заполнения")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле текст статьи обязательно для заполнения")]
        public string Description { get; set; }
        public string Tag { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
