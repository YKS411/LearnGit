using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBlog.Models
{
    public class LoginView
    {
        [Display(Name="用户名")]
        [Required(ErrorMessage ="请输入用户名"),MaxLength(100)]
        public string UserName { get; set; }

        [Display(Name ="密码")]
        [Required(ErrorMessage ="请输入密码"),MaxLength(128)]
        public string PassWord { get; set; }

        //[Required(ErrorMessage = "请输入验证码"), MaxLength(128)]
        //[Display(Name = "验证码")]
        //public string ValidateCode { get; set; }
    }
}
