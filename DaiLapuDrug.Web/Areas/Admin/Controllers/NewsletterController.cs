using DaiLapuDrug.Web.Areas.Admin.Models;
using DaiLapuDrug.Web.Data;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsletterController : Controller
    {
        private readonly MailChimpManager mailChimpManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IHostEnvironment env;

        public NewsletterController(MailChimpManager mailChimpManager, ApplicationDbContext dbContext, IHostEnvironment env)
        {
            this.mailChimpManager = mailChimpManager;
            this.dbContext = dbContext;
            this.env = env;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleCampaign(int articleId)
        {
            var article = dbContext.Articles.FirstOrDefault(x => x.Id == articleId);
            if (article == null)
            {
                return new NotFoundResult();
            }

            var lists = await mailChimpManager.Lists.GetAllAsync();
            var defaultList = lists.FirstOrDefault(x => x.Name == "Day Lapu Drug");

            if (defaultList == null)
                throw new Exception("Default Mail list not found");

            var campaignSettings = new Setting
            {
                ReplyTo = "yegor.androsov@gmail.com",
                FromName = "Day Lapu Drug",
                Title = "Новости от \"Дай лапу, друг\"",
                SubjectLine = "Новости от \"Дай лапу, друг\"",
            };

            var campaign = await mailChimpManager.Campaigns.AddAsync(new Campaign
            {
                Settings = campaignSettings,
                Recipients = new Recipient { ListId = defaultList.Id },
                Type = CampaignType.Regular
            });

            var htmlTemplatePath = env.ContentRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "email-templates"
                + Path.DirectorySeparatorChar.ToString()
                + "new-article.html";

            var htmlTemplate = System.IO.File.ReadAllText(htmlTemplatePath);

            htmlTemplate = htmlTemplate.Replace("{{body}}", article.Body);

            var timeStr = DateTime.Now.ToString();
            var content = await mailChimpManager.Content.AddOrUpdateAsync(campaign.Id,
                new ContentRequest()
                {
                    Html = htmlTemplate,
                });


            return RedirectToAction(nameof(CampaignDetails), new { id = campaign.Id });
        }

        public async Task<IActionResult> CampaignDetails(string id)
        {
            var campaign = await mailChimpManager.Campaigns.GetAsync(id);

            if (campaign == null)
            {
                return NotFound();
            }

            var content = await mailChimpManager.Content.GetAsync(id);
            var vm = new NewsletterCampaignViewModel()
            {
                Id = id,
                Content = content.Html
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CampaignSend(string id)
        {
            await mailChimpManager.Campaigns.SendAsync(id);

            return View();
        }
    }
}
