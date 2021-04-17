using AutoMapper;
using DaiLapuDrug.Web.Areas.Admin.Models;
using DaiLapuDrug.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pet, PetViewModel>(MemberList.Destination)
                .ForMember(x => x.TypeOptionId, cfg => cfg.Ignore())
                .ForMember(x => x.SizeOptionId, cfg => cfg.Ignore())
                .ForMember(x => x.ColorOptionId, cfg => cfg.Ignore())
                .ForMember(x => x.PersonalityOptionIds, cfg => cfg.Ignore())
                .ForMember(x => x.StatusOptionIds, cfg => cfg.Ignore())
                .ForMember(x => x.BreedOptionId, cfg => cfg.Ignore())
                .ForMember(x => x.HairOptionId, cfg => cfg.Ignore())
                .ForMember(x => x.TypeOptions, cfg => cfg.Ignore())
                .ForMember(x => x.BreedOptions, cfg => cfg.Ignore())
                .ForMember(x => x.SizeOptions, cfg => cfg.Ignore())
                .ForMember(x => x.ColorOptions, cfg => cfg.Ignore())
                .ForMember(x => x.HairOptions, cfg => cfg.Ignore())
                .ForMember(x => x.StatusOptions, cfg => cfg.Ignore())
                .ForMember(x => x.PersonalityOptions, cfg => cfg.Ignore())
                .ReverseMap();

            CreateMap<Article, ArticleViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<Option, SelectListItem>()
                .ForMember(x => x.Value, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Text, cfg => cfg.MapFrom(x => x.Value))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Option, TagViewModel>()
                .ForMember(x => x.Value, cfg => cfg.MapFrom(x => x.Id))
                .ForMember(x => x.Tag, cfg => cfg.MapFrom(x => x.Value));

            CreateMap<Option, OptionViewModel>().ReverseMap();
        }
    }
}
