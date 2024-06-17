using Microsoft.Extensions.Hosting;
using MyBlog.BLL.Models.UserModels;
using MyBlog.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models.AccountModel
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
        public MainViewModel User { get; set; }
        public string Author {  get; set; }
        [Required(ErrorMessage = "Поле комментарий обязательно для заполнения")] 
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
