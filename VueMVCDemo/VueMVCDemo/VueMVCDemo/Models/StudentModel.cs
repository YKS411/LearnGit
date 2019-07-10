using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VueMVCDemo.Models
{
    public class StudentModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name ="学生ID")]
        public int Id { get; set; }

        [Required,MaxLength(30),Display(Name ="学生名字")]
        public string Name { get; set; }

        [Required,Display(Name="性别")]
        public bool Sex { get; set; }

        [Display(Name="年龄")]
        public int Age { get; set; }

    }
}
