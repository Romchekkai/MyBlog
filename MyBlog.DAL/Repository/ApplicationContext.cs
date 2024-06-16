using Microsoft.EntityFrameworkCore;
using MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    public class ApplicationContext: DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet <CommentEntity> Comments { get; set; }
        public DbSet <TagEntity> Tags { get; set; }
        public DbSet<UserRole> Roles { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
       : base(options)
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentEntity>()
            .HasOne(u => u.User)
            .WithMany().OnDelete(DeleteBehavior.Restrict);





            modelBuilder.Entity<UserRole>().HasData(
                    new UserRole (1,"Admin"),
                    new UserRole (2,"User"),
                    new UserRole (3,"Moderator")
            );
        }
    }
}
