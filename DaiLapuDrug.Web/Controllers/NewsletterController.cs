using MailChimp;
using MailChimp.Net;
using MailChimp.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly MailChimpManager mailChimpManager;

        public NewsletterController(MailChimpManager mailChimpManager)
        {
            this.mailChimpManager = mailChimpManager;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string email)
        {
            var lists = await mailChimpManager.Lists.GetAllAsync();
            var defaultList = lists.FirstOrDefault(x => x.Name == "Day Lapu Drug");

            if (defaultList == null)
                throw new Exception("Default Mail list not found");

            var member = new Member
            {
                EmailAddress = email,
                StatusIfNew = Status.Subscribed
            };

            await mailChimpManager.Members.AddOrUpdateAsync(defaultList.Id, member);

            return new EmptyResult();
        }
    }
}
