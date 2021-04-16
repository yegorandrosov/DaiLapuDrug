using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class FileAttachment
    {
        public int Id { get; set; }

        public string StorageUrl { get; set; }

        public string Extension { get; set; }

        public string BlobName { get; set; }

        public string ContainerName { get; set; }

        public string OriginalName { get; set; }
    }
}
