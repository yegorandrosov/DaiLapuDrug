using DaiLapuDrug.Web.Data.Entities;

namespace DaiLapuDrug.Web.Areas.Admin.Models
{
    public class OptionViewModel
    {
        public int Id { get; set; }

        public EOptionType Type { get; set; }

        public string Value { get; set; }
    }
}
