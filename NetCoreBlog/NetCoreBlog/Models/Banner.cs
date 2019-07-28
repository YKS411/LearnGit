using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class Banner
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name ="轮播图ID")]
        public int ID { get; set; }

        [MaxLength(50),Display(Name ="标题")]
        public string Text_info { get; set; }

        [Display(Name ="轮播图")]
        [DataType(DataType.Url)]
        public string Img { get; set; }

        [Display(Name ="上传图片")]
        public IFormFile ImgPath { get; set; }

        [Display(Name ="图片连接")]
        [MaxLength(100)]
        [DataType(DataType.Url)]
        public string Link_url { get; set; }

        [Display(Name ="是否时active")]
        public bool Is_active { get; set; }
    }
}
