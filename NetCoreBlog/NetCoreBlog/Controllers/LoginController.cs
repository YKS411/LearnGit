using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Models;
using Microsoft.AspNetCore.Authentication;

namespace NetCoreBlog.Controllers
{
    public class LoginController : Controller
    {
        private readonly NetCoreBlogContext _context;

        public LoginController(NetCoreBlogContext context)
        {
            _context = context;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginView loginview,string returnUrl)
        {
            if (ModelState.IsValid) { 
                var password = Convert.ToBase64String(SHA1.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginview.PassWord + UserInfo.Salt)));
                //var userInfo = _context.UserInfo.FirstOrDefault(s => s.UserName == loginview.UserName && s.PassWord == password);
                var userInfo = _context.UserInfo.FirstOrDefault(s => s.UserName == loginview.UserName);

                if (userInfo == null)
                {
                    ModelState.AddModelError("UserName", "用户不存在，请核对后再登录");
                    return View(loginview);
                }
                if (userInfo.PassWord != password)
                {
                    ModelState.AddModelError("PassWord", "密码错误");
                    return View(loginview);
                }

                var claimIdentity = new ClaimsIdentity("Cookie");
            claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userInfo.ID.ToString()));
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, userInfo.UserName));
            claimIdentity.AddClaim(new Claim(ClaimTypes.Role, userInfo.UserType.ToString()));
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            //登录
            HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(returnUrl ?? $"~/?r={Guid.NewGuid()}");
            }
            return View(loginview);
            // return Redirect(returnUrl ?? $"~/?r={Guid.NewGuid()}");
        }


        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect($"~/?r={Guid.NewGuid()}");
        }



        
        public ActionResult AccessDenied()
        {
            return View();
        }


        //用户注册
        //GET: Register
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Register(string Name,string PassWord,string PassWord2)
        public ActionResult Register([Bind("Name,PassWord,PassWord2")] Register registers)
        {
            //得到post的form数据
            //string name = Request.Form["Name"];
            //string password = Request.Form["PassWord"];
            //string password2 = Request.Form["PassWord2"];

            if (ModelState.IsValid)
            {
                var userName = _context.UserInfo.FirstOrDefault(a => a.UserName == registers.Name);
                if (userName != null)
                {
                    ModelState.AddModelError("Name", "用户名已存在！");
                    return View(registers);
                }
                if (registers.PassWord != registers.PassWord2)
                {
                    ModelState.AddModelError("PassWord", "两次输入的密码不一致！");
                    return View(registers);
                }

                var user = _context.UserInfo.Add(new UserInfo()
                {
                    CreatedAt = DateTime.Now,
                    UserName = registers.Name,
                    UserType = UserType.用户,
                    PassWord = Convert.ToBase64String(SHA1.Create().ComputeHash(System.Text.Encoding.UTF8
                                  .GetBytes(registers.PassWord + UserInfo.Salt)))
                });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
             
            return View(registers);
        }

    }
}