using System.ComponentModel.DataAnnotations;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class ArticleCreateRequest
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Поле заголовок обязательно для заполнения")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле текст статьи обязательно для заполнения")]
        public string Description { get; set; }
      //  public string Tag { get; set; }
        public List<TagEditRequest> Tags { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
