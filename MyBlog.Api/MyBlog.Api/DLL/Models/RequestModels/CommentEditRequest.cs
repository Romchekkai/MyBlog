using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Models.RequestModels
{
    public class CommentEditRequest
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public Guid UserId { get; set; }
        public Guid? ArticleId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
