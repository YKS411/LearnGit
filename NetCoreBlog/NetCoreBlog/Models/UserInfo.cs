using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class UserInfo
    {
        public const string Salt = "AlexYang";
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name="用户ID")]
        public Guid ID { get; set; }

        [Required,MaxLength(100),Display(Name ="用户名")]
        public string UserName { get; set; }

        [Required,MaxLength(128),Display(Name ="密码")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Required,Display(Name ="用户类型")]
        public UserType UserType { get; set; }

        [Display(Name ="创建时间")]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name ="用户头像")]
        public string UserImage { get; set; }  //存储用户头像在服务器，没有在数据库

        public IFormFile ImagePath { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }


    public enum UserType
    {
        用户=0,
        管理员=1,
    }

}
