using DaiLapuDrug.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Models
{
    public class EditOptionsViewModel
    {
        public List<OptionViewModel> Options { get; set; }

        public EOptionType Type { get; set; }
    }
}
