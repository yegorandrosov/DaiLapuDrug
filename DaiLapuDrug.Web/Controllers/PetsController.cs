using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using DaiLapuDrug.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Controllers
{
    public class PetsController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public PetsController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            var pet = applicationDbContext.Pets
                .Include(x => x.PetFileAttachments)
                .ThenInclude(x => x.FileAttachment)
                .Include(x => x.PetOptions)
                .ThenInclude(x => x.Option)
                .Include(x => x.Articles)
                .FirstOrDefault();

            if (pet == null)
                return NotFound();

            Func<EOptionType, string> getOptionValue = type => pet.PetOptions.FirstOrDefault(x => x.Option.Type == type)?.Option.Value;
            Func<EOptionType, string> concatOptionValue = type => string.Join(", ", pet.PetOptions.Where(x => x.Option.Type == type).Select(x => x.Option.Value));

            var vm = new PetDetailsViewModel()
            {
                AnimalId = pet.AnimalId,
                Description = pet.Description,
                Age = "6 месяцев", // todo: implement age calculations
                Breed = getOptionValue(EOptionType.PetBreed),
                Type = getOptionValue(EOptionType.PetType),
                Color = getOptionValue(EOptionType.PetColor),
                Hair = getOptionValue(EOptionType.PetHairType),
                Personality = concatOptionValue(EOptionType.PetPersonality),
                Size = getOptionValue(EOptionType.PetSize),
                Status = concatOptionValue(EOptionType.PetStatus),
                SubType = getOptionValue(EOptionType.PetSubType),
                IsSterilized = pet.IsSterilized,
                Name = pet.Name,
                ImageUrls = pet.PetFileAttachments.Select(x => x.FileAttachment.Url).ToList(),
                Posts = pet.Articles.Select(x => new PetPostViewModel()
                {
                    Body = x.Body,
                    Date = x.CreatedAt.ToString("dd/MM/yyyy")
                }).ToList()
            };

            return View(vm);
        }
    }
}
