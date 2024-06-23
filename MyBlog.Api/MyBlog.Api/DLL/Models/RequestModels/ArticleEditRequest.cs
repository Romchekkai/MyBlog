using System.ComponentModel.DataAnnotations;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class ArticleEditRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле заголовок обязательно для заполнения")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле текст статьи обязательно для заполнения")]
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<TagEditRequest> Tags { get; set; }
        public DateTime DatePosted { get; set; }
        public List<CommentCreateRequest> Comments { get; set; }

    }
}
