using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UEditor.Core;


namespace NetCoreBlog.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name = "文章ID")]
        public int ID { get; set; }

        [MaxLength(70), Display(Name = "标题")]
        public string Title { get; set; }

        [MaxLength(200), Display(Name = "摘要")]
        public string Excerpt { get; set; }

        [Display(Name = "文章分类ID")]
        [ForeignKey("Category")]
        public int? CategoryID
        { get; set; }
        [Display(Name = "文章分类")]
        public virtual Category Category
        {
            get; set;
        }

        [Display(Name = "文章图片")]
        public string Img { get; set; }

        [Display(Name ="上传文章图片")]
        public IFormFile ImgPath { get; set; }


        [DataType(DataType.Html)]
        [Display(Name = "文章内容")]
        public string Content { get; set; }
        

        [Display(Name = "作者ID")]
        [ForeignKey("UserInfo")]
        public Guid UserInfoID { get; set; }
        [Display(Name = "作者")]
        public virtual UserInfo UserInfo
        {
            get; set;
        }

        [DefaultValue(0)]
        [Display(Name = "阅读量")]
        public uint Views { get; set; } = 0;  //默认值为0

        [Display(Name = "推荐位ID")]
        [ForeignKey("Recommend")]
        public int? RecommendID { get; set; }
        [Display(Name = "推荐位")]
        public virtual Recommend Recommend { get; set; }

        [Display(Name = "发布时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "修改时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime ModifiedTime { get; set; }

        //[Display(Name ="评论")]
        //public virtual List<Comment> Comments { get; set; }

        //many to many
        [Display(Name = "标签")]
        public virtual List<TagRelateArticle> TagRelateArticles{get;set;}
    }
}
