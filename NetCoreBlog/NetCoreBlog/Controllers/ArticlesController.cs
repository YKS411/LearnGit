using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;
using UEditor.Core;
using X.PagedList;


namespace NetCoreBlog.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly NetCoreBlogContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly UEditorService _uEditorService;

        //支持的图片类型
        string[] ImageType = { ".png", ".jpg", ".gif", ".jpeg", ".bmp" };

        public ArticlesController(NetCoreBlogContext context,IHostingEnvironment hostingEnvironment,UEditorService uEditorService)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

            _uEditorService = uEditorService;
        }

        // GET: Articles
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Index(int page=1,int pageSize=8)
        {
            if (User.IsInRole("管理员"))
            {
                ViewData["PageSize"] = pageSize;
                var netCoreBlogContext = _context.Article.Include(a => a.Category).Include(a => a.Recommend).Include(a => a.UserInfo);
                return View(await netCoreBlogContext.ToPagedListAsync(page,pageSize));
            }
            if (User.IsInRole("用户"))
            {
                var userIdentityName = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
                var Articles = _context.Article.Include(a => a.Category).Include(a => a.Recommend).Include(a => a.UserInfo)
                    .Where(a => a.UserInfoID.ToString() == userIdentityName);
                return View(await Articles.ToPagedListAsync(page,pageSize));
            }

            return Content("请先登录");
        }

        // GET: Articles/Details/5
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Category)
                .Include(a => a.Recommend)
                .Include(a => a.UserInfo)
                .FirstOrDefaultAsync(m => m.ID == id);


            if (article == null)
            {
                return NotFound();
            }


            var comment = _context.Comment.Include(a => a.UserInfo).Where(s => s.ArticleID == id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["Comments"] = comment;


            article.Views = article.Views + 1;
            _context.Update(article);
            await _context.SaveChangesAsync();

            return View(article);
        }

        //[HttpGet]
        //public JsonResult Get()
        //{
        //    var article = _context.Article;

        //    return Json(article);
        //}

        // GET: Articles/Create
        [Authorize(Roles ="管理员,用户")]
        public IActionResult Create()
        {
            var userIdentityName = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;

            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Name");
            ViewData["RecommendID"] = new SelectList(_context.Set<Recommend>(), "ID", "Name");
            ViewData["UserInfoID"] = new SelectList(_context.UserInfo.Where(u => u.ID.ToString() == userIdentityName), "ID", "UserName");
            //ViewData["UserInfoID"] = new SelectList(_context.Set<UserInfo>(), "ID", "UserName");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Create([Bind("ID,Title,Excerpt,CategoryID,Img,ImgPath,Content,UserInfoID,Views,RecommendID,CreatedAt,ModifiedTime")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreatedAt = DateTime.Now;
                article.ModifiedTime = DateTime.Now;
                _context.Add(article);

                if (article.ImgPath != null)
                {
                    //获取上传图片的后缀，看是否符合标准
                    string typeStr = Path.GetExtension(article.ImgPath.FileName).ToLower();
                    if (!ImageType.Contains(typeStr))
                    {
                        ModelState.AddModelError("ImgPath", "文件格式不支持，目前支持png,gif,jpg,jpeg");
                        return View(article);
                    }

                    string fileName = Path.Combine(_hostingEnvironment.WebRootPath, "ArticlePicture", article.UserInfoID.ToString(), article.ID.ToString() + typeStr);
                    string dirname = Path.Combine(_hostingEnvironment.WebRootPath, "ArticlePicture", article.UserInfoID.ToString(), article.ID.ToString());

                    if (!Directory.Exists(Path.GetDirectoryName(dirname)))
                    {

                        Directory.CreateDirectory(Path.GetDirectoryName(dirname));
                    }

                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        article.ImgPath.CopyTo(stream);
                    }
                    article.Img = Path.Combine("ArticlePicture", article.UserInfoID.ToString(), article.ID.ToString() + typeStr);
                    //article.Img = fileName.Remove(0, 54).ToLower();
                }
              
 
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Name", article.CategoryID);
            ViewData["RecommendID"] = new SelectList(_context.Set<Recommend>(), "ID", "Name", article.RecommendID);
            ViewData["UserInfoID"] = new SelectList(_context.Set<UserInfo>(), "ID", "UserName", article.UserInfoID); 
            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Name", article.CategoryID);
            ViewData["RecommendID"] = new SelectList(_context.Set<Recommend>(), "ID", "Name", article.RecommendID);
            ViewData["UserInfoID"] = new SelectList(_context.Set<UserInfo>(), "ID", "UserName", article.UserInfoID);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Excerpt,CategoryID,Img,ImgPath,Content,UserInfoID,Views,RecommendID,ModifiedTime,CreatedAt")] Article article)
        {
            if (id != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //如果用户没有上传文章图片，保留原来的图片
                    if (article.ImgPath == null){
                        article.Img = article.Img; //没用懒得修改
                    }
                    else { 
                    //获取上传图片的后缀，符合标准允许上传
                    string typeStr = Path.GetExtension(article.ImgPath.FileName).ToLower();
                    if (!ImageType.Contains(typeStr))
                    {
                        ModelState.AddModelError("ImgPath", "图片格式不支持，目前只支持png,gif,jpg,jepg");
                            return View(article);
                    }

                    //这样每一篇文章的图片对应自己图片，避免替换时，上个图片还留在文件中，产生垃圾
                    string fileName = Path.Combine(_hostingEnvironment.WebRootPath, "ArticlePicture", article.UserInfoID.ToString(), article.ID.ToString() + typeStr);

                    string dirname= Path.Combine(_hostingEnvironment.WebRootPath, "ArticlePicture", article.UserInfoID.ToString(), article.ID.ToString());

                        if (!Directory.Exists(dirname))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(dirname));
                    }

                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        article.ImgPath.CopyTo(stream);
                    }
                        //article.Img = fileName.Remove(0, 54).ToLower(); 
                        article.Img=Path.Combine("ArticlePicture", article.UserInfoID.ToString(), article.ID.ToString() + typeStr);
                    }

                    article.ModifiedTime = DateTime.Now;
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Name", article.CategoryID);
            ViewData["RecommendID"] = new SelectList(_context.Set<Recommend>(), "ID", "Name", article.RecommendID);
            ViewData["UserInfoID"] = new SelectList(_context.Set<UserInfo>(), "ID", "UserName", article.UserInfoID);
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Category)
                .Include(a => a.Recommend)
                .Include(a => a.UserInfo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="管理员,用户")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            _context.Article.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ID == id);
        }
    }
}
