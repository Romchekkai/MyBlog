using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Entities
{
    public class CommentEntity
    {
        public Guid Id { get; set; } 
        public string? Text { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid? ArticleId { get; set; }
        public ArticleEntity? Article { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Author { get; set; }
    }
}
