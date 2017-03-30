using ImageStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace nightowlsign.data.Models.Image
{
    public class ImageManager : IImageManager
    {
        public ImageManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }


        public List<ImagesAndSign> Get(ImagesAndSign entity)
        {
            var ret = new List<ImagesAndSign>();
            using (var db = new nightowlsign_Entities())
            {
                ret = db.ImagesAndSigns.OrderBy(x => x.Model).ThenBy(x => x.Caption).ToList<ImagesAndSign>();
            }
            if (!string.IsNullOrEmpty(entity.Caption))
            {
                ret = ret.FindAll(p => p.Caption.ToLower().StartsWith(entity.Caption));
            }
            if (entity.SignSize > 0)
            {
                ret = ret.FindAll(p => p.SignSize.Equals(entity.SignSize));
            }

            return ret;
        }

        public data.Image Find(int imageId)
        {
            using (var db = new nightowlsign_Entities())
            {
                return db.Images.Find(imageId);
            }
        }

        public bool Validate(data.Image entity)
        {
            ValidationErrors.Clear();

            if (!string.IsNullOrEmpty(entity.Caption))
            {
                //if (entity.Caption.ToLower() == entity.Caption)
                //{
                //    ValidationErrors.Add(new KeyValuePair<string, string>("Caption", "Caption cannot be all lower case"));
                //}

            }
            return (ValidationErrors.Count == 0);
        }

        public bool Validate(UploadedImage entity)
        {
            ValidationErrors.Clear();

            if (!string.IsNullOrEmpty(entity.Caption))
            {
                if (entity.Caption.ToLower() == entity.Caption)
                {
                    //     ValidationErrors.Add(new KeyValuePair<string, string>("Caption", "Caption cannot be all lower case"));
                }
            }
            return (ValidationErrors.Count == 0);
        }

        public bool Update(UploadedImage imageToUpDate)
        {
            if (Validate(imageToUpDate))
            {
                try
                {
                   var entity = new data.Image()
                    {
                        Id = imageToUpDate.Id,
                        Caption = imageToUpDate.Caption,
                        DateTaken = imageToUpDate.DateTaken,
                        SignSize = imageToUpDate.SignId
                    };
                    using (var db = new nightowlsign_Entities())
                    {
                        db.Images.Attach(entity);
                        var modifiedImage = db.Entry(entity);
                        modifiedImage.Property(e => e.Caption).IsModified = true;
                        modifiedImage.Property(e => e.DateTaken).IsModified = true;
                        modifiedImage.Property(e => e.SignSize).IsModified = true;
                        // modifiedImage.Property(e => e.ImageURL).IsModified = true;
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    return false;
                }
            }
            return false;
        }


        public async Task<bool> Insert(string fileName, UploadedImage imageToUpload)
        {
            try
            {
                var entity = new data.Image
                {
                    Caption = fileName,
                    ImageURL = imageToUpload.Url,
                    ThumbNailLarge = ImageService.ImageToByte(imageToUpload.Thumbnails[1].Bitmap),
                    ThumbNailSmall = ImageService.ImageToByte(imageToUpload.Thumbnails[0].Bitmap),
                    DateTaken = imageToUpload.DateTaken,
                    SignSize = imageToUpload.SignId
                };
                if (Validate(entity))
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        db.Images.Add(entity);
                        await db.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return false;
            }
            return false;
        }


        public bool Delete(data.Image entity)
        {
            using (var db = new nightowlsign_Entities())
            {
                db.Images.Attach(entity);
                db.Images.Remove(entity);
                db.SaveChanges();
                return true;
            }
        }
    }
}
