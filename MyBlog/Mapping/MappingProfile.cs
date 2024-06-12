using AutoMapper;
using MyBlog.BLL.Models.UserModels;
using MyBlog.DAL.Entities;
using MyBlog.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map userEntity, userModel, userViewModels
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserModel, UserEntity>();

            CreateMap<UserModel, LoginViewModel>();
            CreateMap<LoginViewModel, UserModel>();

            CreateMap<UserModel, RegisterViewModel>()
                .ForMember(x => x.PasswordConfirm, opt => opt.Ignore());
            CreateMap<RegisterViewModel, UserModel>();

            CreateMap<UserModel, MainViewModel>();
            CreateMap<MainViewModel, UserModel>();

            CreateMap<UserModel, UserEditView>();
            CreateMap<UserEditView, UserModel>();

            //Map articleEntity, articleModel, postViewModels

            CreateMap<ArticleEntity, ArticleModel>().ForMember(d => d.Tags, opt => opt.MapFrom(i => i.Tags)); 
            CreateMap<ArticleModel, ArticleEntity>().ForMember(d => d.Tags,opt=>opt.MapFrom(i=>i.Tags));

            CreateMap<ArticleModel, PostViewModel>();
            CreateMap<PostViewModel, ArticleModel>();

            CreateMap<ArticleModel, CreateArticleView>();
            CreateMap<CreateArticleView, ArticleModel>();

            //Map commentEntity, commentModel, commentViewModels
            CreateMap<CommentEntity, CommentModel>();
            CreateMap<CommentModel, CommentEntity>();
            CreateMap<CommentViewModel, CommentModel>();
            CreateMap<CommentModel, CommentViewModel>();


            //Map tag

            CreateMap<TagEntity, TagModel>();
            CreateMap<TagModel, TagEntity>();

            CreateMap<TagModel, TagViewModel>();
            CreateMap<TagViewModel, TagModel>();
        }
    }
}
//ForMember(m=>m.ModelRole, opt=>opt.MapFrom(scr=>scr.Role));