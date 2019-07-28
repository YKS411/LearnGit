using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class Register
    {
        [Display(Name="请输入用户名")]
        public string Name { get; set; }

        [Display(Name ="请输入密码")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name ="请再次输入密码")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string PassWord2 { get; set; }
    }
}
