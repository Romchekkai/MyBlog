using MyBlog.BLL.Models.UserModels;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Extensions
{
    public static class TagsConvert
    {
        public static List<TagEntity> Convert(this List<TagEntity> entities, List<TagModel> models )
        {
            foreach ( var model in models )
            {
                entities.Add(new TagEntity() { Name = model.Name });
            }
            return entities;
        }
    }
}
