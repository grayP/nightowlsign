using System.Collections.Generic;

namespace nightowlsign.data.Models.ScheduleStore
{
    public interface IScheduleStoreViewModel
    {
        data.Schedule Schedule { get; set; }
        List<StoreSelect> AllStores { get; set; }
        List<int?> Schedulestore { get; set; }
        int SignSize { get; set; }
        string Mode { get; set; }
        string EventCommand { get; set; }
        string EventArgument { get; set; }
        bool IsDetailVisible { get; set; }
        bool IsSearchVisible { get; set; }
        bool IsListAreaVisible { get; set; }
        bool IsValid { get; set; }
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        void HandleRequest();
        void LoadData(int signId);
    }
}