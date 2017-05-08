using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using nightowlsign.data.Models.Image;

namespace nightowlsign.data.Models.ScheduleImage
{
    public class ScheduleImageManager : IScheduleImageManager
    {
        public ScheduleImageManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<int?> Get(data.Schedule entity)
        {
            using (var db = new nightowlsign_Entities())
            {
                var query = (from c in db.ScheduleImages
                             where c.ScheduleID == entity.Id
                             select c.ImageID);
                return query.ToList();
            }
        }

        public List<ImageSelect> GetAllImages(int signId, int scheduleId)
        {
            using (var db = new nightowlsign_Entities())
            {
                var query = (from s in db.Images.Where(s => s.SignSize == signId)
                             select new ImageSelect()
                             {
                                 ImageId = s.Id,
                                 Name = s.Caption,
                                 ThumbNail = s.ThumbNailLarge,
                                 SignId = signId,
                                 SignSize = s.SignSize ?? 0
                             });
                return query.ToList();
            }
        }

        public List<ImageSelect> GetAllImages(int scheduleId)
        {
            using (var db = new nightowlsign_Entities())
            {
                var query = (from s in db.ScheduleImages
                             join i in db.Images on s.ImageID equals i.Id
                             where s.ScheduleID == scheduleId
                             select new ImageSelect()
                             {
                                 ImageId = i.Id,
                                 Name = i.Caption,
                                 ThumbNail = i.ThumbNailLarge,
                                 SignSize = i.SignSize ?? 0,
                                 Selected = true
                             });
                return query.ToList();
            }
        }

        public data.Image Find(int id)
        {
            using (var db = new nightowlsign_Entities())
            {
                return db.Images.FirstOrDefault(e => e.Id == id);
            }
        }

        public void RemoveImagesFromScheduleImage(int imageId)
        {
            using (var db = new nightowlsign_Entities())
            {
                var ret = db.ScheduleImages.Where(e => e.ImageID == imageId);
                db.ScheduleImages.RemoveRange(ret);
                db.SaveChanges();
            }
        }

        public void UpdateImageList(ImageSelect imageSelect, data.Schedule schedule)
        {
            using (var db = new nightowlsign_Entities())
            {
                var imageSelected = new data.ScheduleImage
                {
                    ImageID = imageSelect.ImageId,
                    ScheduleID = schedule.Id,
                    Id = imageSelect.Id
                };
                if (imageSelect.Selected)
                {
                    db.Set<data.ScheduleImage>().AddOrUpdate(imageSelected);
                    db.SaveChanges();
                }
                else
                {
                    data.ScheduleImage scheduleImage =
                        db.ScheduleImages.Find(imageSelect.Id);
                    if (scheduleImage != null)
                    {
                        db.ScheduleImages.Attach(scheduleImage);
                        db.ScheduleImages.Remove(scheduleImage);
                        db.SaveChanges();
                    }
                }
            }
        }

        internal data.ScheduleImage GetValues(ImageSelect imageSelect)
        {

            using (var db = new nightowlsign_Entities())
            {
                return db.ScheduleImages.FirstOrDefault(x => x.ImageID == imageSelect.ImageId && x.ScheduleID == imageSelect.ScheduleId);
            }
        }

        internal bool IsSelected(int scheduleId, int imageId)
        {
            using (var db = new nightowlsign_Entities())
            {
                return db.ScheduleImages.Any(x => x.ScheduleID == scheduleId && x.ImageID == imageId);
            }
        }
    }
}
