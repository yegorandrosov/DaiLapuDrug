using AutoMapper;
using DaiLapuDrug.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public NewsController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }


        public IActionResult Index()
        {
            var news = applicationDbContext.Articles
                .ToList();

            return View(news);
        }

        [Route("/news/{slug:string}")]
        public IActionResult News(string slug)
        {
            var news = applicationDbContext.Articles.Where(x => x.Slug == slug).FirstOrDefault();

            if (news == null)
                return NotFound();

            return View(news);
        }
    }
}
