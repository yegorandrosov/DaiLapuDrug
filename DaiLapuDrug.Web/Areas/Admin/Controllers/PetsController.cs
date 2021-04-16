using AutoMapper;
using AutoMapper.QueryableExtensions;
using DaiLapuDrug.Web.Areas.Admin.Models;
using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public PetsController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var pets = applicationDbContext.Pets.Where(x => x.IsDeleted == false)
                .Select(x => new PetViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsPublished = x.IsPublished,
                });

            return View(pets);
        }

        public ActionResult Create()
        {
            var viewModel = new PetViewModel();

            viewModel.IsPublished = true;

            FillViewModel(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetViewModel model)
        {
            var pet = mapper.Map<Pet>(model);
            pet.CreatedAt = DateTime.Now;

            pet.Slug = pet.Name.GenerateSlug();

            SaveViewModel(model, pet);

            applicationDbContext.Pets.Add(pet);
            applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var pet = applicationDbContext.Pets
                .Include(x => x.PetOptions)
                .ThenInclude(x => x.Option)
                .FirstOrDefault(x => x.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            var viewModel = mapper.Map<PetViewModel>(pet);

            FillViewModel(viewModel, pet);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetViewModel model)
        {
            var pet = applicationDbContext.Pets
                .Include(x => x.PetOptions)
                .FirstOrDefault(x => x.Id == model.Id);

            if (pet == null)
            {
                return NotFound();
            }

            mapper.Map(model, pet);

            pet.UpdatedAt = DateTime.Now;
            pet.Slug = pet.Name.GenerateSlug();

            SaveViewModel(model, pet);

            applicationDbContext.Update(pet);
            applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            var pet = applicationDbContext.Pets.FirstOrDefault(x => x.Id == id);

            if (pet == null)
            {
                return NotFound();
            }

            var viewModel = mapper.Map<PetViewModel>(pet);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PetViewModel model)
        {
            var pet = applicationDbContext.Pets.FirstOrDefault(x => x.Id == model.Id);

            if (pet == null)
            {
                return NotFound();
            }

            pet.DeletedAt = DateTime.Now;
            pet.IsDeleted = true;

            applicationDbContext.Update(pet);
            applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public void FillViewModel(PetViewModel petViewModel, Pet existingPet = null)
        {
            var allOptions = applicationDbContext.Options.ToList();

            Func<EOptionType, List<SelectListItem>> mappingFunc = e => mapper.Map<List<SelectListItem>>(allOptions.Where(x => x.Type == e).ToList());
            Func<EOptionType, List<TagViewModel>> mappingFunc1 = e => mapper.Map<List<TagViewModel>>(allOptions.Where(x => x.Type == e).ToList());

            petViewModel.HairOptions = mappingFunc(EOptionType.PetHairType);
            petViewModel.BreedOptions = mappingFunc(EOptionType.PetBreed);
            petViewModel.ColorOptions = mappingFunc(EOptionType.PetColor);
            petViewModel.TypeOptions = mappingFunc(EOptionType.PetType);
            petViewModel.SizeOptions = mappingFunc(EOptionType.PetSize);

            petViewModel.StatusOptions = mappingFunc1(EOptionType.PetStatus);
            petViewModel.PersonalityOptions = mappingFunc1(EOptionType.PetPersonality);

            if (existingPet != null)
            {
                petViewModel.BreedOptionId = existingPet.PetOptions.FirstOrDefault(x => x.Option.Type == EOptionType.PetBreed)?.OptionId;
                petViewModel.ColorOptionId = existingPet.PetOptions.FirstOrDefault(x => x.Option.Type == EOptionType.PetColor)?.OptionId;
                petViewModel.HairOptionId = existingPet.PetOptions.FirstOrDefault(x => x.Option.Type == EOptionType.PetHairType)?.OptionId;
                petViewModel.TypeOptionId = existingPet.PetOptions.FirstOrDefault(x => x.Option.Type == EOptionType.PetType)?.OptionId;
                petViewModel.SizeOptionId = existingPet.PetOptions.FirstOrDefault(x => x.Option.Type == EOptionType.PetSize)?.OptionId;

                petViewModel.StatusOptionIds = string.Join(",", existingPet.PetOptions.Where(x => x.Option.Type == EOptionType.PetStatus).Select(x => x.OptionId));
                petViewModel.PersonalityOptionIds = string.Join(",", existingPet.PetOptions.Where(x => x.Option.Type == EOptionType.PetPersonality).Select(x => x.OptionId));
            }
        }

        public void SaveViewModel(PetViewModel petViewModel, Pet pet)
        {
            foreach (var petOption in pet.PetOptions)
            {
                applicationDbContext.PetOptions.Remove(petOption);
            }

            var petOptionIds = new List<int?>()
            {
                petViewModel.BreedOptionId,
                petViewModel.ColorOptionId,
                petViewModel.TypeOptionId,
                petViewModel.SizeOptionId,
                petViewModel.HairOptionId
            };

            if (!string.IsNullOrWhiteSpace(petViewModel.PersonalityOptionIds))
            {
                petOptionIds.AddRange(petViewModel.PersonalityOptionIds.Split(",").Select(x => (int?)Convert.ToInt32(x)));
            }

            if (!string.IsNullOrWhiteSpace(petViewModel.StatusOptionIds))
            {
                petOptionIds.AddRange(petViewModel.StatusOptionIds.Split(",").Select(x => (int?)Convert.ToInt32(x)));
            }

            foreach (var petOptionId in petOptionIds.Where(x => x.GetValueOrDefault() != 0))
            {
                applicationDbContext.Add(new PetOption()
                {
                    OptionId = petOptionId.Value,
                    Pet = pet
                });
            };
        }
    }
}
