using AutoMapper;
using DaiLapuDrug.Web.Areas.Admin.Models;
using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticlesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext applicationDbContext;

        public ArticlesController(IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            this.mapper = mapper;
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var articles = applicationDbContext.Articles.Where(x => x.IsDeleted == false && x.PetId == null)
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Body = x.Body,
                    PetId = x.PetId,
                    IsPublished = x.IsPublished,
                });

            return View(articles);
        }

        public IActionResult Create(int? petId = null)
        {
            var viewModel = new ArticleViewModel()
            {
                PetId = petId,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleViewModel model)
        {
            var article = mapper.Map<Article>(model);

            article.CreatedAt = DateTime.Now;
            article.Slug = SlugGenerator.SlugGenerator.GenerateSlug(article.Title);

            applicationDbContext.Articles.Add(article);
            applicationDbContext.SaveChanges();

            if (model.PetId != null)
            {
                return RedirectToAction("Edit", "Pets", new { id = model.PetId });
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var article = applicationDbContext.Articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            var viewModel = mapper.Map<ArticleViewModel>(article);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel model)
        {
            var article = applicationDbContext.Articles.FirstOrDefault(x => x.Id == model.Id);

            if (article == null)
            {
                return NotFound();
            }

            mapper.Map(model, article);

            article.UpdatedAt = DateTime.Now;
            article.Slug = SlugGenerator.SlugGenerator.GenerateSlug(article.Title);

            applicationDbContext.Articles.Update(article);
            applicationDbContext.SaveChanges();

            if (model.PetId != null)
            {
                return RedirectToAction("Edit", "Pets", new { id = model.PetId });
            }

            return RedirectToAction(nameof(Index));
        }


        public ActionResult Delete(int id)
        {
            var article = applicationDbContext.Articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            var viewModel = mapper.Map<ArticleViewModel>(article);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ArticleViewModel model)
        {
            var article = applicationDbContext.Articles.FirstOrDefault(x => x.Id == model.Id);

            if (article == null)
            {
                return NotFound();
            }

            article.DeletedAt = DateTime.Now;
            article.IsDeleted = true;

            applicationDbContext.Update(article);
            applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
