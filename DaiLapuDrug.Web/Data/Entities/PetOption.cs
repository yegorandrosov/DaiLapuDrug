namespace DaiLapuDrug.Web.Data.Entities
{
    public class PetOption
    {
        public int Id { get; set; }

        public int OptionId { get; set; }

        public virtual Option Option { get; set; }

        public int PetId { get; set; }

        public virtual Pet Pet { get; set; }
    }
}
