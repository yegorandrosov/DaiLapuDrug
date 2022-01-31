using DaiLapuDrug.Web.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Controllers
{
    public class StaticController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public StaticController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [Route("/static/page/{slug}")]
        public IActionResult Page(string slug)
        {
            var staticPage = applicationDbContext.Articles
                .FirstOrDefault(x => x.Type == Data.Entities.EArticleType.Static && x.Slug == slug);

            if (staticPage == null)
                return NotFound();

            return View(staticPage);
        }
    }
}
