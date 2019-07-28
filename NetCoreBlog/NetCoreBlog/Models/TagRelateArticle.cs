using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class TagRelateArticle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="多对多ID")]
        [Key]
        public int ID { get; set; }

        [ForeignKey("Article")]
        public int ArticleID { get; set; }
        public Article Article { get; set; }

        [ForeignKey("Tag")]
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
