using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;
using Newtonsoft.Json;
using X.PagedList;

namespace NetCoreBlog.Controllers
{
    public class TagsController : Controller
    {
        private readonly NetCoreBlogContext _context;

        public TagsController(NetCoreBlogContext context)
        {
            _context = context;
        }

        // GET: Tags
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tag.ToListAsync());
        }

        // GET: Tags/Details/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Details(int? id,int page=1)
        {
            int pageSize = 10;

            if (id == null)
            {
                return NotFound();
            }

            //var articles = _context.Article.Include(a => a.Category).Include(a => a.Recommend).Include(a => a.UserInfo)
            //    .Include(t => t.TagRelateArticles).ThenInclude(a => a.Tag).Where(a=>a.ID==id);

            var tag = await _context.Tag
                .FirstOrDefaultAsync(m => m.ID == id);

            //通过tagReleastArticle表查询tagId=id的数据，关联article所有数据。
            var articles = _context.TagRelateArticle.Include(a => a.Article).ThenInclude(a => a.Category)
                                                   .Include(a => a.Article).ThenInclude(a => a.Recommend)
                                                   .Include(a => a.Article).ThenInclude(a => a.UserInfo)
                                                   .Where(a => a.TagID == id).ToPagedList(page,pageSize);

            if (articles==null)
            {
                return Content("没有文章");
            }

            ViewData["Articles"] = articles;
            if (tag == null)
            {
                return NotFound();
            }

            //var tagList = new
            //{
            //    //articles = articles,
            //    tag = tag
            //};

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             //return Content(JsonConvert.SerializeObject(tagList,settings));
           // return Json(new { articles = articles});

            return View(tag);
        }

        // GET: Tags/Create
        [Authorize(Roles = "管理员")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Create([Bind("ID,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Tag tag)
        {
            if (id != tag.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.ID))
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
            return View(tag);
        }

        // GET: Tags/Delete/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tag.FindAsync(id);
            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _context.Tag.Any(e => e.ID == id);
        }
    }
}
