using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;
using X.PagedList;

namespace NetCoreBlog.Controllers
{
    public class TagRelateArticlesController : Controller
    {
        private readonly NetCoreBlogContext _context;

        public TagRelateArticlesController(NetCoreBlogContext context)
        {
            _context = context;
        }

        // GET: TagRelateArticles
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Index(int page=1)
        {
            int pageSize = 10;
            var netCoreBlogContext = _context.TagRelateArticle.Include(t => t.Article).Include(t => t.Tag);
            return View(await netCoreBlogContext.ToPagedListAsync(page,pageSize));
        }

        // GET: TagRelateArticles/Details/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagRelateArticle = await _context.TagRelateArticle
                .Include(t => t.Article)
                .Include(t => t.Tag)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tagRelateArticle == null)
            {
                return NotFound();
            }

            return View(tagRelateArticle);
        }

        // GET: TagRelateArticles/Create
        [Authorize(Roles = "管理员")]
        public IActionResult Create()
        {
            ViewData["ArticleID"] = new SelectList(_context.Article, "ID", "Title");
            ViewData["TagID"] = new SelectList(_context.Tag, "ID", "Name");
            return View();
        }

        // POST: TagRelateArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Create([Bind("ID,ArticleID,TagID")] TagRelateArticle tagRelateArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tagRelateArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleID"] = new SelectList(_context.Article, "ID", "Title", tagRelateArticle.ArticleID);
            ViewData["TagID"] = new SelectList(_context.Tag, "ID", "Name", tagRelateArticle.TagID);
            return View(tagRelateArticle);
        }

        // GET: TagRelateArticles/Edit/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagRelateArticle = await _context.TagRelateArticle.FindAsync(id);
            if (tagRelateArticle == null)
            {
                return NotFound();
            }
            ViewData["ArticleID"] = new SelectList(_context.Article, "ID", "Title", tagRelateArticle.ArticleID);
            ViewData["TagID"] = new SelectList(_context.Tag, "ID", "Name", tagRelateArticle.TagID);
            return View(tagRelateArticle);
        }

        // POST: TagRelateArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ArticleID,TagID")] TagRelateArticle tagRelateArticle)
        {
            if (id != tagRelateArticle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tagRelateArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagRelateArticleExists(tagRelateArticle.ID))
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
            ViewData["ArticleID"] = new SelectList(_context.Article, "ID", "Title", tagRelateArticle.ArticleID);
            ViewData["TagID"] = new SelectList(_context.Tag, "ID", "Name", tagRelateArticle.TagID);
            return View(tagRelateArticle);
        }

        // GET: TagRelateArticles/Delete/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagRelateArticle = await _context.TagRelateArticle
                .Include(t => t.Article)
                .Include(t => t.Tag)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tagRelateArticle == null)
            {
                return NotFound();
            }

            return View(tagRelateArticle);
        }

        // POST: TagRelateArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tagRelateArticle = await _context.TagRelateArticle.FindAsync(id);
            _context.TagRelateArticle.Remove(tagRelateArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagRelateArticleExists(int id)
        {
            return _context.TagRelateArticle.Any(e => e.ID == id);
        }
    }
}
