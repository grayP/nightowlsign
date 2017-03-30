using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageStorage;

namespace nightowlsign.data.Models.Image
{
    public interface IImageManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        List<ImagesAndSign> Get(ImagesAndSign Entity);
        data.Image Find(int imageId);
        bool Validate(data.Image entity);
        bool Validate(UploadedImage entity);
        bool Update(UploadedImage imageToUpDate);
        Task<Boolean> Insert(string fileName, UploadedImage imageToUpload);
        bool Delete(data.Image entity);
    }
}