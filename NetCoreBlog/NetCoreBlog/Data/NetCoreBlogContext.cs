using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;

namespace NetCoreBlog.Models
{
    public class NetCoreBlogContext : DbContext
    {
        public NetCoreBlogContext (DbContextOptions<NetCoreBlogContext> options)
            : base(options)
        {
        }

        public DbSet<NetCoreBlog.Models.Article> Article { get; set; }

        public DbSet<NetCoreBlog.Models.Banner> Banner { get; set; }

        public DbSet<NetCoreBlog.Models.Category> Category { get; set; }

        public DbSet<NetCoreBlog.Models.Comment> Comment { get; set; }

        public DbSet<NetCoreBlog.Models.Link> Link { get; set; }

        public DbSet<NetCoreBlog.Models.Recommend> Recommend { get; set; }

        public DbSet<NetCoreBlog.Models.Tag> Tag { get; set; }

        public DbSet<NetCoreBlog.Models.UserInfo> UserInfo { get; set; }

        public DbSet<NetCoreBlog.Models.TagRelateArticle> TagRelateArticle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //级联删除
            //modelBuilder.Entity<Comment>().HasOne(k => k.Article).WithMany(u => u.Comments)
            //   .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Comment>().HasOne(k => k.UserInfo).WithMany(u => u.Comments)
            //    .OnDelete(DeleteBehavior.Cascade);

            //忽略IFormFile类型
            modelBuilder.Entity<UserInfo>().Ignore(k => k.ImagePath);

            //忽略article的IFormFile类型
            modelBuilder.Entity<Article>().Ignore(k => k.ImgPath);

            //忽略Banner的IFormFile类型
            modelBuilder.Entity<Banner>().Ignore(k => k.ImgPath);

            base.OnModelCreating(modelBuilder);
        }
    }
}
