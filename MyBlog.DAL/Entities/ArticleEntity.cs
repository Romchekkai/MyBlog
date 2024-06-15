using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Entities
{
    public class ArticleEntity
    {
        public Guid Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public List<TagEntity> Tags { get; set; } 
        public DateTime? DatePosted { get; set; } 
        public List<CommentEntity> Comments { get; set; } = new();
    }
}
