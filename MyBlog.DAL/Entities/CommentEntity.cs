using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Entities
{
    public class CommentEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Text { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid ArticleId { get; set; }
        public ArticleEntity Article { get; set; }
    }
}
