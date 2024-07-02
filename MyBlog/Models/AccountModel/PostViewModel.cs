using MyBlog.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.AccountModel
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле заголовок обязательно для заполнения")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле текст статьи обязательно для заполнения")]
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public MainViewModel User { get; set; }
        public string Tag { get; set; }

        public TagViewModel modelTag { get; set; }

        public CommentViewModel commentViewModel { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; } 
        public DateTime DatePosted { get; set; }
        public List<CommentViewModel> Comments { get; set; }

    }
}
