using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Areas.Admin.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string PreviewPicture { get; set; }

        public string PreviewBody { get; set; }
        public bool IsPublished { get; set; } 
        public int? PetId { get; set; }
    }
}
