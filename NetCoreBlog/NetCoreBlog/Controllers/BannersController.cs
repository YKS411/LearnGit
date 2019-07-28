using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace NetCoreBlog.Controllers
{
    public class BannersController : Controller
    {
        private readonly NetCoreBlogContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        string[] ImageType = { ".jpg", ".png", ".gif", ".jpeg",".bmp" };

        public BannersController(NetCoreBlogContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Banners
        [Authorize(Roles ="管理员")]
        public async Task<IActionResult> Index(int page=1)
        {
            int pageSize = 10;
            return View(await _context.Banner.ToPagedListAsync(page,pageSize));
        }

        // GET: Banners/Details/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner
                .FirstOrDefaultAsync(m => m.ID == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Banners/Create
        [Authorize(Roles = "管理员")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Banners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Create([Bind("ID,Text_info,ImgPath,Link_url,Is_active")] Banner banner)
        {
            if (ModelState.IsValid)
            {
                //获取上传图片的后缀
                string TypeStr = Path.GetExtension(banner.ImgPath.FileName).ToLower();
                if (!ImageType.Contains(TypeStr))
                {
                    ModelState.AddModelError("ImgPath", "文件格式不支持，目前只支持png,gif,jpg,jpeg,bmp");
                    return View(banner);
                }
                string fileName = Path.Combine(_hostingEnvironment.WebRootPath, "BannerPicture", banner.ID.ToString() + TypeStr);
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                using(var stream=new FileStream(fileName, FileMode.Create))
                {
                    banner.ImgPath.CopyTo(stream);
                }

                banner.Img= Path.Combine("BannerPicture", banner.ID.ToString() + TypeStr);
                //banner.Img = fileName.Remove(0,54).ToLower();
                _context.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Banners/Edit/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Text_info,ImgPath,Link_url,Is_active")] Banner banner)
        {
            if (id != banner.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //如果没有修改轮播图片,保留原来的图片。
                    if (banner.ImgPath == null)
                    {
                        banner.Img = banner.Img;
                    }
                    else
                    {
                        string TypeStr = Path.GetExtension(banner.ImgPath.FileName).ToLower();
                        if (!ImageType.Contains(TypeStr))
                        {
                            ModelState.AddModelError("ImgPath", "文件格式不支持，目前只支持png,gif,jpg,jpeg,bmp");
                            return View(banner);
                        }

                        string fileName = Path.Combine(_hostingEnvironment.WebRootPath, "BannerPicture", banner.ID.ToString() + TypeStr);
                        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                        }
                        using (var stream=new FileStream(fileName, FileMode.Create))
                        {
                            banner.ImgPath.CopyTo(stream);
                        }
                        banner.Img = Path.Combine("BannerPicture", banner.ID.ToString() + TypeStr);
                        //banner.Img = fileName.Remove(0, 54).ToLower();
                    }
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.ID))
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
            return View(banner);
        }

        // GET: Banners/Delete/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner
                .FirstOrDefaultAsync(m => m.ID == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banner.FindAsync(id);
            System.IO.File.Delete(_hostingEnvironment.WebRootPath+banner.Img);
            _context.Banner.Remove(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banner.Any(e => e.ID == id);
        }
    }
}
