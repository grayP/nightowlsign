using ImageStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightowlsign.data;


namespace nightowlsign.data.Models.Images
{
    public class ImageManager
    {
        public ImageManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }


        public List<Image> Get(Image Entity)
        {
            List<Image> ret = new List<Image>();
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.Images.OrderBy(x => x.DateTaken.Value).ToList<Image>();
            }
            if (!string.IsNullOrEmpty(Entity.Caption))
            {
                ret = ret.FindAll(p => p.Caption.ToLower().StartsWith(Entity.Caption));
            }
            return ret;
        }

        public Image Find(int ImageID)
        {
            Image ret = null;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.Images.Find(ImageID);
            }
            return ret;

        }

        public bool Validate(Image entity)
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
                    ValidationErrors.Add(new KeyValuePair<string, string>("Caption", "Caption cannot be all lower case"));
                }

            }
            return (ValidationErrors.Count == 0);

        }


        public Boolean Update(UploadedImage imageToUpDate)
        {
            bool ret = false;
            if (Validate(imageToUpDate))
            {
                try
                {
                    Image entity = new Image()
                    {
                        Id = imageToUpDate.Id,
                        Caption = imageToUpDate.Caption,
                        DateTaken = imageToUpDate.DateTaken,
                        SignSize=imageToUpDate.SignId
                    };
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        db.Images.Attach(entity);
                        var modifiedImage = db.Entry(entity);
                        modifiedImage.Property(e => e.Caption).IsModified = true;
                        modifiedImage.Property(e => e.DateTaken).IsModified = true;
                        modifiedImage.Property(e => e.SignSize).IsModified = true;
                        // modifiedImage.Property(e => e.ImageURL).IsModified = true;
                        db.SaveChanges();
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    ret = false;
                }
            }

            return ret;
        }


        public async Task<Boolean> Insert(UploadedImage imageToUpload)
        {
            bool ret = false;
            try
            {
                Image entity = new Image();
                entity.Caption = imageToUpload.Caption;
                entity.ImageURL = imageToUpload.Url;
                entity.ThumbNailLarge = ImageService.ImageToByte(imageToUpload.Thumbnails[1].bitmap);
                entity.ThumbNailSmall = ImageService.ImageToByte(imageToUpload.Thumbnails[0].bitmap);
                entity.DateTaken = imageToUpload.DateTaken;
                entity.SignSize = imageToUpload.SignId;
                ret = Validate(entity);
                if (ret)
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        db.Images.Add(entity);
                        await db.SaveChangesAsync();
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return ret;
            }
        }


        public bool Delete(Image entity)
        {
            bool ret = false;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                db.Images.Attach(entity);
                db.Images.Remove(entity);
                db.SaveChanges();
                ret = true;
            }
            return ret;

        }
    }
}
