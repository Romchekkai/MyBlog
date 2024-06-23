using AutoMapper;
using MyBlog.Api.DLL.Models.RequestModels;
using MyBlog.Api.DLL.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Api.Contracts.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map userEntity, userModel, userViewModels
            CreateMap<User, RegisterRequest>().ReverseMap();

            //Map articleEntity, articleModel, postViewModels

            CreateMap<Article, ArticleCreateRequest>().ReverseMap();
            CreateMap<Article, ArticleEditRequest>().ReverseMap();

            //Map commentEntity, commentModel, commentViewModels
            CreateMap<Comment, CommentCreateRequest>().ReverseMap();
            CreateMap<Comment, CommentEditRequest>().ReverseMap();


            //Map tag

            CreateMap<Tag, TagCreateRequest>().ReverseMap();
            CreateMap<Tag, TagEditRequest>().ReverseMap();

            //Map Role

            CreateMap<UserRole, RoleEditRequest>().ReverseMap();

        }
    }
}
