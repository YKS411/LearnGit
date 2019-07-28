using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreBlog.Models;

namespace NetCoreBlog.Controllers
{
    public class RecommendsController : Controller
    {
        private readonly NetCoreBlogContext _context;

        public RecommendsController(NetCoreBlogContext context)
        {
            _context = context;
        }

        // GET: Recommends
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recommend.ToListAsync());
        }

        // GET: Recommends/Details/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommend
                .FirstOrDefaultAsync(m => m.ID == id);
            if (recommend == null)
            {
                return NotFound();
            }

            return View(recommend);
        }

        // GET: Recommends/Create
        [Authorize(Roles = "管理员")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recommends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Create([Bind("ID,Name")] Recommend recommend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recommend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recommend);
        }

        // GET: Recommends/Edit/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommend.FindAsync(id);
            if (recommend == null)
            {
                return NotFound();
            }
            return View(recommend);
        }

        // POST: Recommends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Recommend recommend)
        {
            if (id != recommend.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recommend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecommendExists(recommend.ID))
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
            return View(recommend);
        }

        // GET: Recommends/Delete/5
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommend
                .FirstOrDefaultAsync(m => m.ID == id);
            if (recommend == null)
            {
                return NotFound();
            }

            return View(recommend);
        }

        // POST: Recommends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理员")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recommend = await _context.Recommend.FindAsync(id);
            _context.Recommend.Remove(recommend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecommendExists(int id)
        {
            return _context.Recommend.Any(e => e.ID == id);
        }
    }
}
