using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models.UserModels
{
    public class ArticleModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public string Tag { get; set; }
        public List<TagModel> Tags { get; set; } 
        public DateTime? DatePosted { get; set; }
        public List<CommentModel>? Comments { get; set; } = new();
    }
}
