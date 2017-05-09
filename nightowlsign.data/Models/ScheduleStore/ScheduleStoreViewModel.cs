using System.Collections.Generic;

namespace nightowlsign.data.Models.ScheduleStore
{
    public class ScheduleStoreViewModel : BaseModel.ViewModelBase, IScheduleStoreViewModel
    {
        private readonly IScheduleStoreManager _scheduleStoreManager;
        public ScheduleStoreViewModel(IScheduleStoreManager scheduleStoreManager) : base()
        {
            _scheduleStoreManager = scheduleStoreManager;
            Schedule = new data.Schedule();
            AllStores = new List<StoreSelect>();
        }

        public ScheduleStoreViewModel()
        {
            
        }


        public data.Schedule Schedule { get; set; }
        public List<StoreSelect> AllStores { get; set; }
        public List<int?> Schedulestore { get; set; }
        public int SignSize { get; set; }
    

        protected override void Init()
        {

            base.Init();
        }

        public override void HandleRequest()
        {
            base.HandleRequest();
        }

        public void LoadData(int signId)
        {
            SignSize = signId;
            Get();
        }

        protected override void Get()
        {
             AllStores = _scheduleStoreManager.GetAllStores(SignSize);
            foreach (StoreSelect ss in AllStores)
            {
                ss.ScheduleId = Schedule.Id;
                data.ScheduleStore selected = _scheduleStoreManager.GetValues(ss);
                if (selected != null)
                {
                    ss.Selected = true;
                    ss.Id = selected.Id;
                    ss.StoreId = selected.StoreId ?? 0;
                }
               // ss.Selected = sm.IsSelected(Schedule.Id, ss.Id);
            }
        }
    }
}
