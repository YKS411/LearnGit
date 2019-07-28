using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;
using X.PagedList;


namespace NetCoreBlog.Controllers
{
    public class UserInfoesController : Controller
    {
        //支持上传图片类型
        string[] ImageType={".png",".jpg",".gif",".jpeg",".bmp"};

        private readonly NetCoreBlogContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserInfoesController(NetCoreBlogContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: UserInfoes
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Index(int page=1, int pageSize=10)
        {
            if (User.IsInRole("管理员"))
            {
                ViewData["PageSize"] = pageSize; //每页最多显示的记录数的返回给视图。以便在下次查询和排序的时候，每页也显示相同的记录数。

                var userInfo = _context.UserInfo;

                return View(await userInfo.AsNoTracking().ToPagedListAsync(page, pageSize));
            }
            if (User.IsInRole("用户"))
            {
                var userIdentityName = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;

                var userInfo = await _context.UserInfo.Where(s => s.ID.ToString() == userIdentityName).ToListAsync();

                return View(await userInfo.ToPagedListAsync(page, pageSize));
            }

            return Content("没有登录");
        }


        //public async Task<IActionResult> Index()
        //{
        //    if (User.IsInRole("管理员"))
        //    {
        //        return View(await _context.UserInfo.ToListAsync());
        //    }
        //    if (User.IsInRole("用户"))
        //    {
        //        var userIdentityName = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;

        //        var userInfo = await _context.UserInfo.Where(s => s.ID.ToString() == userIdentityName).ToListAsync();

        //        return View(userInfo);
        //    }

        //    return Content("没有登录");
        //}

        // GET: UserInfoes/Details/5
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Create
        [Authorize(Roles = "管理员")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Create([Bind("ID,UserName,PassWord,UserType,ImagePath")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserInfo.FirstOrDefault(a => a.UserName == userInfo.UserName);

                if(user != null)
                {
                    ModelState.AddModelError("UserName", "用户名已存在，请重新选择");
                    return View(userInfo);
                }

                userInfo.ID = Guid.NewGuid();
                userInfo.CreatedAt = DateTime.Now;

                //var ImagePaths = Request.Form.Keys;
                //var ImagePath = ImagePaths.FirstOrDefault();

                //获取图片后缀，判定上传是否为图片类型
                string typeStr = Path.GetExtension(userInfo.ImagePath.FileName).ToLower();

                if (!ImageType.Contains(typeStr))
                {
                    ModelState.AddModelError("ImagePath", "文件格式不支持，目前支持png,gif,jpg,jpeg");
                    return View(userInfo);
                }

                string fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UserPictures", userInfo.ID.ToString(), DateTime.Now.ToString("yyyy-MMddHHmmsss") + typeStr);

                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }

                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    userInfo.ImagePath.CopyTo(stream);
                }

                userInfo.UserImage=Path.Combine("UserPictures", userInfo.ID.ToString(), DateTime.Now.ToString("yyyy-MMddHHmmsss") + typeStr);
                //userInfo.UserImage = fileName.Remove(0, 54).ToLower();

                //对密码进行加盐取哈希
                userInfo.PassWord = Convert.ToBase64String(SHA1.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(userInfo.PassWord+UserInfo.Salt)));
                _context.Add(userInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Edit/5
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,UserName,PassWord,UserType,CreatedAt,UserImage")] UserInfo userInfo)
        {
            if (id != userInfo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //对密码进行加盐取哈希
                    userInfo.PassWord = Convert.ToBase64String(SHA1.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(userInfo.PassWord + UserInfo.Salt)));
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        [Authorize(Roles ="管理员")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UserInfo
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="管理员")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userInfo = await _context.UserInfo.FindAsync(id);

            //顺便把用户头像也给删除
            System.IO.File.Delete(_hostingEnvironment.WebRootPath+userInfo.UserImage);
            Directory.Delete((_hostingEnvironment.WebRootPath+userInfo.UserImage).Substring(0,104));  //同时删除文件夹；每个用户图片对应一个文件夹，用户修改图片时，会存在多个图片。
            
            _context.UserInfo.Remove(userInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(Guid id)
        {
            return _context.UserInfo.Any(e => e.ID == id);
        }
    }
}
