using System.ComponentModel.DataAnnotations;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class TagEditRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле тэг обязательно для заполнения")]
        public string Name { get; set; }
        public Guid ArticleId { get; set; }
    }
}
