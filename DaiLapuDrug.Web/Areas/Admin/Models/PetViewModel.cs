using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaiLapuDrug.Web.Areas.Admin.Models
{
    public class PetViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string AnimalId { get; set; }

        public DateTime? EstimatedDateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsPublished { get; set; }

        public bool IsSterilized { get; set; }

        public int? TypeOptionId { get; set; }

        public int? SubTypeOptionId { get; set; }

        public int? BreedOptionId { get; set; }

        public int? SizeOptionId { get; set; }

        public int? ColorOptionId { get; set; }

        public int? HairOptionId { get; set; }

        public string PersonalityOptionIds { get; set; }

        public string StatusOptionIds { get; set; }

        public List<SelectListItem> TypeOptions { get; set; }

        public List<SelectListItem> SubTypeOptions { get; set; }

        public List<SelectListItem> BreedOptions { get; set; }

        public List<SelectListItem> SizeOptions { get; set; }

        public List<SelectListItem> ColorOptions { get; set; }

        public List<SelectListItem> HairOptions { get; set; }

        public List<TagViewModel> PersonalityOptions { get; set; }

        public List<TagViewModel> StatusOptions { get; set; }
    }

    public class PetListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<OptionViewModel> Options { get; set; }
    }
}
