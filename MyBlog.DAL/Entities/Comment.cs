using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
    }
}
