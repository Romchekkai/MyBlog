using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models.UserModels
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
    }
}
