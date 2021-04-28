using AutoMapper;
using DaiLapuDrug.Web.Areas.Admin.Models;
using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using DaiLapuDrug.Web.Services;
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
        private readonly FileAttachmentService fileAttachmentService;

        public PetsController(ApplicationDbContext applicationDbContext, IMapper mapper, FileAttachmentService fileAttachmentService)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.fileAttachmentService = fileAttachmentService;
        }

        public ActionResult Index()
        {
            var pets = applicationDbContext.Pets.Where(x => x.IsDeleted == false)
                .Include(x => x.PetOptions)
                .ThenInclude(x => x.Option)
                .Select(x => new PetListItemViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Options = mapper.Map<List<OptionViewModel>>(x.PetOptions.Select(x => x.Option).ToList())
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

        public ActionResult GetPetPosts(int petId)
        {
            var articles = applicationDbContext.Articles.Where(x => x.PetId == petId)
                .Select(x => new PetPostListItemViewModel()
            {
                Id = x.Id,
                Body = x.Body,
                CreatedAt = x.CreatedAt
            });

            return PartialView("_PetPosts", articles);
        }

        public ActionResult GetPetFileAttachments(int petId)
        {
            var attachments = applicationDbContext.PetFileAttachments
                .Where(x => x.PetId == petId)
                .Select(x => new PetFileAttachmentListItemViewModel()
            {
                Id = x.Id,
                Name = x.FileAttachment.OriginalName,
                OriginalUrl = x.FileAttachment.Url,
                PreviewUrl = x.FileAttachment.ThumbUrl,
                IsImage = x.FileAttachment.IsImage,
                IsCover = x.IsCover
            });

            ViewData["PetId"] = petId;

            return PartialView("_PetFileAttachments", attachments);
        }

        [HttpPost]
        public ActionResult UpdateCoverImage(int petId, int petFileAttachmentId)
        {
            var petFileAttachments = applicationDbContext.PetFileAttachments.Where(x => x.PetId == petId).ToList();
            var newCover = petFileAttachments.FirstOrDefault(x => x.Id == petFileAttachmentId);

            petFileAttachments.ForEach(x => 
            { 
                x.IsCover = false; 
                applicationDbContext.PetFileAttachments.Update(x); 
            });

            newCover.IsCover = true;

            applicationDbContext.SaveChanges();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeletePetFileAttachment(int id)
        {
            var toDelete = applicationDbContext.PetFileAttachments.FirstOrDefault(x => x.Id == id);
            
            if (toDelete.IsCover)
            {
                var anyOther = applicationDbContext.PetFileAttachments.FirstOrDefault(x => x.Id != toDelete.Id && x.PetId == toDelete.PetId);

                anyOther.IsCover = true;

                applicationDbContext.PetFileAttachments.Update(anyOther);
            }

            applicationDbContext.PetFileAttachments.Remove(toDelete);
            applicationDbContext.SaveChanges();

            return new EmptyResult();
        }

        [HttpPost]
        public async Task<ActionResult> UploadPetFileAttachments(int petId)
        {
            var attachments = await fileAttachmentService.UploadFromRequest("pets");

            foreach (var attachment in attachments)
            {
                var petFileAttachment = new PetFileAttachment()
                {
                    FileAttachment = attachment,
                    PetId = petId,
                };

                applicationDbContext.PetFileAttachments.Add(petFileAttachment);
            }

            await applicationDbContext.SaveChangesAsync();

            return new EmptyResult();
        }

        public void FillViewModel(PetViewModel petViewModel, Pet existingPet = null)
        {
            var allOptions = applicationDbContext.Options.ToList();

            Func<EOptionType, List<SelectListItem>> mappingFunc = 
                e => mapper.Map<List<SelectListItem>>(allOptions.Where(x => x.Type == e).ToList());
            Func<EOptionType, List<TagViewModel>> mappingFunc1 = 
                e => mapper.Map<List<TagViewModel>>(allOptions.Where(x => x.Type == e).ToList());

            petViewModel.HairOptions = mappingFunc(EOptionType.PetHairType);
            petViewModel.BreedOptions = mappingFunc(EOptionType.PetBreed);
            petViewModel.ColorOptions = mappingFunc(EOptionType.PetColor);
            petViewModel.TypeOptions = mappingFunc(EOptionType.PetType);
            petViewModel.SubTypeOptions = mappingFunc(EOptionType.PetSubType);
            petViewModel.SizeOptions = mappingFunc(EOptionType.PetSize);

            petViewModel.StatusOptions = mappingFunc1(EOptionType.PetStatus);
            petViewModel.PersonalityOptions = mappingFunc1(EOptionType.PetPersonality);

            if (existingPet != null)
            {
                petViewModel.BreedOptionId = existingPet.PetOptions
                    .FirstOrDefault(x => x.Option.Type == EOptionType.PetBreed)?.OptionId;
                petViewModel.ColorOptionId = existingPet.PetOptions
                    .FirstOrDefault(x => x.Option.Type == EOptionType.PetColor)?.OptionId;
                petViewModel.HairOptionId = existingPet.PetOptions
                    .FirstOrDefault(x => x.Option.Type == EOptionType.PetHairType)?.OptionId;
                petViewModel.TypeOptionId = existingPet.PetOptions
                    .FirstOrDefault(x => x.Option.Type == EOptionType.PetType)?.OptionId;
                petViewModel.SubTypeOptionId = existingPet.PetOptions
                    .FirstOrDefault(x => x.Option.Type == EOptionType.PetSubType)?.OptionId;
                petViewModel.SizeOptionId = existingPet.PetOptions
                    .FirstOrDefault(x => x.Option.Type == EOptionType.PetSize)?.OptionId;

                petViewModel.StatusOptionIds = string.Join(",", existingPet.PetOptions
                    .Where(x => x.Option.Type == EOptionType.PetStatus).Select(x => x.OptionId));
                petViewModel.PersonalityOptionIds = string.Join(",", existingPet.PetOptions
                    .Where(x => x.Option.Type == EOptionType.PetPersonality).Select(x => x.OptionId));
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
                petViewModel.HairOptionId,
                petViewModel.SubTypeOptionId
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
