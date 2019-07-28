using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class Recommend
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name ="推荐文章ID")]
        public int ID { get; set; }

        [MaxLength(100)]
        [Display(Name ="推荐位")]
        public string Name { get; set; }

        public virtual List<Article> Article { get; set; }
    }
}
