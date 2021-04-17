using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class OptionsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext applicationDbContext;

        public OptionsController(IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            this.mapper = mapper;
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(EOptionType type)
        {
            var options = applicationDbContext.Options
                .Where(x => x.Type == type)
                .ProjectTo<OptionViewModel>(mapper.ConfigurationProvider)
                .ToList();

            var viewModel = new EditOptionsViewModel()
            {
                Options = options,
                Type = type,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditOptionsViewModel model)
        {
            model.Options = model.Options.Where(x => x != null).ToList();

            var options = applicationDbContext.Options
                .Where(x => x.Type == model.Type)
                .ToList();

            // added
            foreach (var option in model.Options.Where(x => x.Id == 0))
            {
                var newOption = mapper.Map<Option>(option);
                newOption.Type = model.Type;
                applicationDbContext.Options.Add(newOption);
            }

            // updated
            foreach (var option in model.Options.Where(x => x.Id != 0))
            {
                var oldOption = options.FirstOrDefault(x => x.Id == option.Id);
                if (option.Value != oldOption.Value)
                {
                    oldOption.Value = option.Value;

                    applicationDbContext.Update(oldOption);
                }
            }

            // deleted
            foreach (var option in options)
            {
                if (!model.Options.Any(x => x.Id == option.Id))
                {
                    applicationDbContext.Options.Remove(option);
                }
            }

            applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetOptionPartialView()
        {
            return PartialView("_OptionItem", new OptionViewModel());
        }
    }
}
