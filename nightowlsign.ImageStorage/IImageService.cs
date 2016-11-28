using ImageStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImageStorage

{
        public interface IImageService
        {
            Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file, UploadedImage oldImage);
            Task AddImageToBlobStorageAsync(UploadedImage image);
        }
}
