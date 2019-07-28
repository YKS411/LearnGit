using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetCoreBlog.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name="博客分类ID")]
        public int ID { get; set; }

        [Display(Name ="博客分类")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name ="分类排序")]
        //[MaxLength(100)]
        public int Index { get; set; }

        [Display(Name ="文章")]
        public virtual ICollection<Article> Articles { get; set; }
    }
}
