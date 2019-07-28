using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name ="文章标签ID")]
        public int ID { get; set; }

        [MaxLength(100)]
        [Display(Name ="文章标签")]
        public string Name { get; set; }

        //many to many
        [Display(Name ="文章")]
        public virtual List<TagRelateArticle> TagRelateArticles { get; set; }
    }
}
