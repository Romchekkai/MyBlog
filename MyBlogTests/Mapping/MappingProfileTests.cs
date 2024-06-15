using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.BLL.Models.UserModels;
using MyBlog.DAL.Entities;
using MyBlog.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Mapping.Tests
{
    [TestClass()]
    public class MappingProfileTests
    {
        [TestMethod()]
        public void MappingProfileTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }
        [TestMethod()]
        public void MappingProfile()
        {
            var mapper = new Mapper(new MapperConfiguration(c => { c.AddProfile<MappingProfile>(); }));

          //  var entity = new CommentEntity() {ArticleId= Guid.NewGuid(),Id= Guid.NewGuid(), Text="sfs",UserId =Guid.NewGuid(),CommentsByComment = new List<CommentEntity>() { new CommentEntity() { ArticleId = Guid.NewGuid(), Id = Guid.NewGuid(), Text = "sfs", UserId = Guid.NewGuid() } } };

          // var model= mapper.Map<CommentModel>(entity);
        }
    }
}