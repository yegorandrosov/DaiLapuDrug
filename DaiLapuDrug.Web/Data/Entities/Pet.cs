using System;
using System.Collections.Generic;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        public string AnimalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? EstimatedDateOfBirth { get; set; }

        public string Slug { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool IsSterilized { get; set; }

        public virtual ICollection<PetOption> PetOptions { get; set; }
            = new List<PetOption>();
    }
}
