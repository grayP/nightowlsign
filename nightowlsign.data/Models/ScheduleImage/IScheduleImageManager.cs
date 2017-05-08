using System.Collections.Generic;
using nightowlsign.data.Models.Image;

namespace nightowlsign.data.Models.ScheduleImage
{
    public interface IScheduleImageManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        List<int?> Get(data.Schedule entity);
        List<ImageSelect> GetAllImages(int signId, int scheduleId);
        List<ImageSelect> GetAllImages(int scheduleId);
        data.Image Find(int id);
        void RemoveImagesFromScheduleImage(int imageId);
        void UpdateImageList(ImageSelect imageSelect, data.Schedule schedule);
    }
}