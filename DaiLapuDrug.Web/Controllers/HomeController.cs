using AutoMapper;
using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using DaiLapuDrug.Web.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index() => View();

        public IActionResult Help() => View();

        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            var contactRequest = mapper.Map<ContactRequest>(contactViewModel);

            applicationDbContext.ContactRequests.Add(contactRequest);
            applicationDbContext.SaveChanges();

            return new EmptyResult();
        }

        [Route("/error")]
        public IActionResult Error() => View();
    }
}
