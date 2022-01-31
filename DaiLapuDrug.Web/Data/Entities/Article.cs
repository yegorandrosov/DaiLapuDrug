using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PreviewBody { get; set; }

        public string Body { get; set; }

        public string Slug { get; set; }

        public int? PetId { get; set; }

        public virtual Pet Pet { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsRequired { get; set; }

        public EArticleType Type { get; set; }
    }

    public enum EArticleType
    {
        Default,
        Pet,
        Static
    }
}
