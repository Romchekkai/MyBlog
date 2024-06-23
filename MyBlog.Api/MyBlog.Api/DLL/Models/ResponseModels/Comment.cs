using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Models.ResponseModels
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? ArticleId { get; set; }
        public Article? Article { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
