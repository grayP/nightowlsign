using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace nightowlsign.ImageService

{
        public interface IImageService
        {
            Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file, UploadedImage oldImage);
            Task AddImageToBlobStorageAsync(UploadedImage image);

        }
    
   
}
