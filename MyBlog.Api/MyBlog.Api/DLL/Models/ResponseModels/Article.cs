using MyBlog.Api.DLL.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.DLL.Models.ResponseModels
{
    public class Article
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime? DatePosted { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }
}
