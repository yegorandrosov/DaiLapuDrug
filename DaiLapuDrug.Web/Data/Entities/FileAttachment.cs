using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Data.Entities
{
    public class FileAttachment
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Extension { get; set; }

        public string BlobName { get; set; }

        public string ContainerName { get; set; }

        public string OriginalName { get; set; }

        public bool IsImage { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public string MimeType { get; set; }

        public string ThumbUrl { get; set; }

        public string ThumbExtension { get; set; }

        public string ThumbBlobName { get; set; }

        public string ThumbName { get; set; }

        public int ThumbImageWidth { get; set; }

        public int ThumbImageHeight { get; set; }

        public string ThumbMimeType { get; set; }
    }
}
