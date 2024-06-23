using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class CommentCreateRequest
    {
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Поле комментарий обязательно для заполнения")]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
