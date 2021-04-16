using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class Option
    {
        public int Id { get; set; }

        public EOptionType Type { get; set; }

        public string Value { get; set; }
    }

    public enum EOptionType
    {
        PetType,
        PetBreed,
        PetSize,
        PetColor,
        PetPersonality,
        PetHairType,
        PetStatus,
    }
}
