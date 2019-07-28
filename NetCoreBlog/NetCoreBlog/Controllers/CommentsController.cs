using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;
using X.PagedList;

namespace NetCoreBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly NetCoreBlogContext _context;

        public CommentsController(NetCoreBlogContext context)
        {
            _context = context;
        }

        // GET: Comments
        [Authorize(Roles ="管理员")]
        public async Task<IActionResult> Index(int page=1)
        {
            var netCoreBlogContext = _context.Comment.Include(c => c.UserInfo);

            int pageSize = 10;

            return View(await netCoreBlogContext.ToPagedListAsync(page,pageSize));
        }

        // GET: Comments/Details/5
        [Authorize(Roles ="管理员")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.UserInfo)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [Authorize(Roles = "管理员,用户")]
        public IActionResult Create(int? id)
        {
            //获取当前用户
            var userIdentityName = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Name)?.Value;
            if (userIdentityName == null)
            {
                return RedirectToAction(nameof(LoginController.Index),"Login");
                //return NotFound();
            }
            else
            {
                ViewData["UserInfoID"] = new SelectList(_context.UserInfo.Where(s=>s.UserName==userIdentityName), "ID", "UserName");
                ViewData["RouteArticleId"] = id;
                return View();
            }

        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员,用户")]
        public async Task<IActionResult> Create(int id, [Bind("CommentID,Content,CreatedAt,UserInfoID,ArticleID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;
                comment.ArticleID = id;
 
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Articles",new{id});
            }
            ViewData["UserInfoID"] = new SelectList(_context.UserInfo, "ID", "PassWord", comment.UserInfoID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["UserInfoID"] = new SelectList(_context.UserInfo, "ID", "PassWord", comment.UserInfoID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int id, [Bind("CommentID,Content,CreatedAt,UserInfoID,ArticleID")] Comment comment)
        {
            if (id != comment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentID))
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
            ViewData["UserInfoID"] = new SelectList(_context.UserInfo, "ID", "PassWord", comment.UserInfoID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.UserInfo)
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentID == id);
        }
    }
}
