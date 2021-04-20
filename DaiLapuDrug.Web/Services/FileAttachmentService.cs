using DaiLapuDrug.Web.Data;
using DaiLapuDrug.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Services
{
    public class FileAttachmentService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureBlobStorageService blobStorageService;
        private readonly ApplicationDbContext applicationDbContext;

        public FileAttachmentService(IHttpContextAccessor httpContextAccessor,
            AzureBlobStorageService blobStorageService,
            ApplicationDbContext applicationDbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.blobStorageService = blobStorageService;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<List<FileAttachment>> UploadFromRequest(string containerName)
        {
            var fileAttachments = new List<FileAttachment>();

            foreach (var file in httpContextAccessor.HttpContext.Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var blobName = Guid.NewGuid().ToString().Replace("-", "");
                    var extension = Path.GetExtension(file.FileName);

                    var fileAttachment = new FileAttachment()
                    {
                        BlobName = blobName + extension,
                        Extension = extension,
                        ContainerName = containerName,
                        OriginalName = file.FileName,
                        MimeType = MimeTypes.GetMimeType(extension)
                    };

                    if (fileAttachment.MimeType.StartsWith("image/"))
                    {
                        using var ms = new MemoryStream();
                        var fs = file.OpenReadStream();

                        fs.CopyTo(ms);
                        ms.Position = 0;
                        try
                        {
                            using Image sourceImage = Image.FromStream(ms);
                            fileAttachment.IsImage = true;
                            fileAttachment.ImageHeight = sourceImage.Height;
                            fileAttachment.ImageWidth = sourceImage.Width;

                            ms.Position = 0;

                            using NetVips.Image thumb = NetVips.Image.ThumbnailStream(ms, 126, crop: NetVips.Enums.Interesting.Attention);
                            using var thumbMs = new MemoryStream();
                            thumb.WriteToStream(thumbMs, extension);

                            thumbMs.Position = 0;
                            fileAttachment.ThumbBlobName = blobName + "-thumb" + extension;
                            fileAttachment.ThumbImageHeight = fileAttachment.ThumbImageWidth = 126;
                            fileAttachment.ThumbExtension = extension;
                            fileAttachment.ThumbMimeType = MimeTypes.GetMimeType(fileAttachment.ThumbBlobName);
                            fileAttachment.ThumbUrl = await blobStorageService.UploadFile(containerName, fileAttachment.ThumbBlobName, thumbMs, fileAttachment.ThumbMimeType);
                        }
                        catch
                        {
                        }

                        ms.Position = 0;

                        fileAttachment.Url = await blobStorageService.UploadFile(containerName, fileAttachment.BlobName, ms, fileAttachment.MimeType);
                    }
                    else
                    {
                        fileAttachment.Url = await blobStorageService.UploadFile(containerName, fileAttachment.BlobName, file.OpenReadStream(), fileAttachment.MimeType);
                    }

                    fileAttachments.Add(fileAttachment);

                    applicationDbContext.Add(fileAttachment);
                }
            }

            return fileAttachments;
        }
    }
}
