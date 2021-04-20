namespace DaiLapuDrug.Web.Data.Entities
{
    public class PetFileAttachment
    {
        public int Id { get; set; }

        public int PetId { get; set; }

        public virtual Pet Pet { get; set; }

        public int FileAttachmentId { get; set; }

        public virtual FileAttachment FileAttachment { get; set; }

        public bool IsCover { get; set; }
    }
}
