using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name = "评论ID")]
        public int CommentID { get; set; }

        [Display(Name = "评论内容")]
        public string Content { get; set; }

        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedAt{get;set;}

        [Display(Name ="留言者")]
        [ForeignKey("UserInfo")]
        public Guid UserInfoID { get; set; }
        [Display(Name ="用户信息")]
        public virtual UserInfo UserInfo { get; set; }

        [Display(Name ="文章ID")]
        public int ArticleID { get; set; }

        //don't create a foreign key that will create more than one path to a table in a list of cascading referential actions

        //[Display(Name ="文章ID")]
        //[ForeignKey("Article")]
        //public int? ArticleID { get; set; }
        //[Display(Name ="文章")]
        //public virtual Article Article { get; set; }


    }
}
