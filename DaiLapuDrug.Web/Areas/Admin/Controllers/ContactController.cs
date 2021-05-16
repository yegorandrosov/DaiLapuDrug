using DaiLapuDrug.Web.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ContactController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var contactRequests = dbContext.ContactRequests.OrderByDescending(x => x.CreatedAt);

            return View(contactRequests);
        }

        public IActionResult MarkAsRead(int id)
        {
            var contactRequest = dbContext.ContactRequests.FirstOrDefault(x => x.Id == id);

            if (contactRequest != null)
            {
                contactRequest.IsNew = false;

                dbContext.ContactRequests.Update(contactRequest);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
