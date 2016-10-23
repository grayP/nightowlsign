using System;
using System.Collections.Generic;
using nightowlsign.data.Models.Signs;


namespace nightowlsign.data.Models
{
    public class SendToSignViewModel : BaseModel.ViewModelBase
    {
        public SendToSignViewModel() : base()
        {
            DisplayMessage = "";
            Schedule = new data.Schedule();
            AllImagesInSchedule = new List<ImageSelect>();
        }


        public data.Schedule Schedule { get; set; }
        public List<ImageSelect> AllImagesInSchedule { get; set; }
        public List<SignDto> signsForSchedule { get; set; }
        public SignParameters signParameter { get; set; }

        public string DisplayMessage { get; set; }
        protected override void Init()
        {

            base.Init();
        }

        public override void HandleRequest()
        {
            base.HandleRequest();
        }

        public void loadData()
        {
            Get();
        }

        protected override void Get()
        {

            SendToSignManager sm = new SendToSignManager();
         //   schedulestore = sm.Get(Schedule);
           signsForSchedule = sm.GetSignsForSchedule(Schedule.Id);
           AllImagesInSchedule = sm.GetImagesForThisSchedule(Schedule.Id);
            //foreach (ImageSelect imageSelect in AllImagesInSchedule)
            //{
            //    imageSelect.ScheduleId = Schedule.Id;

            //    ScheduleImage selected = sm.GetValues(imageSelect);
            //    if (selected != null)
            //    {
            //        imageSelect.Selected = true;
            //        imageSelect.Id = selected.Id;
            //        imageSelect.ImageId = selected.ImageID ?? 0;
            //    }

            //}

        }

    }
}
