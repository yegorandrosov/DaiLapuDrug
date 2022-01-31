using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class ArticleFileAttachment
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int FileAttachmentId { get; set; }

        public virtual FileAttachment FileAttachment { get; set; }
    }
}
