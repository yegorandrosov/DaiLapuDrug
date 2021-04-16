using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Controllers
{
    public class OptionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
