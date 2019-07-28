using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class Link
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name ="链接ID")]
        public int ID { get; set; }

        [Display(Name ="链接名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name ="网址")]
        [MaxLength(100)]
        [DataType(DataType.Url)]
        public string URLFiled { get; set; }

    }
}
