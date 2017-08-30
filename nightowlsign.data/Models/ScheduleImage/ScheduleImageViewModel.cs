using System.Collections.Generic;
using System.Linq;
using nightowlsign.data.Models.Image;

namespace nightowlsign.data.Models.ScheduleImage
{
    public class ScheduleImageViewModel : BaseModel.ViewModelBase
    {
        public ScheduleImageViewModel() : base()
        {
            Schedule = new data.Schedule();
            AllImages = new List<ImageSelect>();
        }


        public data.Schedule Schedule { get; set; }
        public int SignId { get; set; }
        public IEnumerable<ImageSelect> AllImages { get; set; }
        public List<int?> ScheduleImage { get; set; }

        protected override void Init()
        {
            base.Init();
        }

        public override void HandleRequest()
        {
            base.HandleRequest();
        }

        public void LoadData()
        {
            Get();
        }

        public void GetImagesForThisSchedule(int scheduleId)
        {
            var sm = new ScheduleImageManager();
            AllImages = sm.GetAllImages(Schedule.Id).OrderBy(s=>s.Name);
         
        }

        protected override void Get()
        {
            var sm = new ScheduleImageManager();
            AllImages = sm.GetAllImages(SignId, Schedule.Id);
            foreach (var imageSelect in AllImages)
            {
                imageSelect.ScheduleId = Schedule.Id;
                var selected = sm.GetValues(imageSelect);
                if (selected != null)
                {
                    imageSelect.Selected = true;
                    imageSelect.Id = selected.Id;
                    imageSelect.ImageId = selected.ImageID ?? 0;
                    imageSelect.SignId = SignId;
                }
            }
        }
    }
}
