using AutoMapper;
using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using DaiLapuDrug.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public HomeController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            var contactRequest = mapper.Map<ContactRequest>(contactViewModel);

            applicationDbContext.ContactRequests.Add(contactRequest);
            applicationDbContext.SaveChanges();

            return new EmptyResult();
        }
    }
}
