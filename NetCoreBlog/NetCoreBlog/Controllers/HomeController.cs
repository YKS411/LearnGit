using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBlog.Models;
using Microsoft.EntityFrameworkCore;


namespace NetCoreBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly NetCoreBlogContext _context;

        public HomeController(NetCoreBlogContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Category.ToList();
            if (categories == null)
            {
                return NotFound();
            }

            var banners = _context.Banner.Where(b => b.Is_active == true).ToList().Take(4);

            var rdname = _context.Recommend.FirstOrDefault(s => s.Name == "首页推荐");
            IEnumerable<Article> recommends;
            if (rdname == null)
            {
                //如果没有推荐，什么都不做
                recommends = null;
            }
            else
            {
                recommends = _context.Article.Where(r => r.RecommendID == rdname.ID).ToList().Take(3);
            }

            //var recommends = _context.Article.Where(r => r.RecommendID == 1).ToList().Take(3);  //默认首页推荐id为1


            var articles = _context.Article.OrderByDescending(x=>x.ID).ToList().Take(10);

            var hot = _context.Article.OrderByDescending(x => x.Views).ToList().Take(10);


            var fdrd1 = _context.Recommend.FirstOrDefault(s => s.Name == "热门推荐");

            IEnumerable<Article> remen;
            if (fdrd1 == null)
            {
                remen = null;
            }
            else
            {
                remen = _context.Article.Where(r => r.RecommendID == fdrd1.ID).ToList().Take(6);
            }

            //var remen = _context.Article.Where(r => r.RecommendID == 2).ToList().Take(6);

            //标签
            var tags = _context.Tag.ToList();
            //友情链接
            var links = _context.Link.ToList();


            ViewData["Banners"] = banners;
            ViewData["categories"] = categories;
            ViewData["Recommends"] = recommends;
            ViewData["Articles"] = articles;
            ViewData["hots"] = hot;
            ViewData["rementui"] = remen;
            ViewData["Tags"] = tags;
            ViewData["Links"] = links;

            return View();
        }

        public IActionResult Search(Search searchs)
        {

            var s = _context.Article.Include(a => a.UserInfo)
                                  .Include(a => a.Category)
                                  .Include(a => a.Recommend)
                                  .Where(a => a.Content.Contains(searchs.search)||a.Title.Contains(searchs.search)) //同时查询文档内容或文档标题包含。
                                  .ToList();
            return View(s);
        }



        public IActionResult home()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
