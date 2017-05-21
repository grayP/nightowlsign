using System;
using System.Collections.Generic;

namespace nightowlsign.data
{
   public partial class StoreAndSign
    {
        //public List<SelectPlayList> PlayLists { get; set; }
        public data.Schedule DefaultSchedule { get; set; }
        public data.Schedule CurrentSchedule { get; set; }
        public List<data.Schedule> AvailableSchedules { get; set; }
        public List<data.Schedule> SelectedSchedules { get; set; }
        public Sign Sign { get; set; }

        public void GetPlayLists()
        {
            throw new NotImplementedException();
        }
    }

}
