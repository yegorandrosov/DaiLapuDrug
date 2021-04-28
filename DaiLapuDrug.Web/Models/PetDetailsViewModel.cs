using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Models
{
    public class PetDetailsViewModel
    {
        public List<string> ImageUrls { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AnimalId { get; set; }

        public string Age { get; set; }

        public bool IsSterilized { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public string Breed { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string Hair { get; set; }

        public string Personality { get; set; }

        public string Status { get; set; }

        public List<PetPostViewModel> Posts { get; set; }
    }

    public class PetPostViewModel
    {
        public string Date { get; set; }

        public string Body { get; set; }
    }
}
