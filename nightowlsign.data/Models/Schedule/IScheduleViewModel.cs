using System.Collections.Generic;

namespace nightowlsign.data.Models.Schedule
{
    public interface IScheduleViewModel
    {
        List<data.ScheduleAndSign> Schedules { get; set; }
        data.Schedule SearchEntity { get; set; }
        data.Schedule Entity { get; set; }
        IEnumerable<SelectListItem> SignList { get; }
        string Mode { get; set; }
        string EventCommand { get; set; }
        string EventArgument { get; set; }
        bool IsDetailVisible { get; set; }
        bool IsSearchVisible { get; set; }
        bool IsListAreaVisible { get; set; }
        bool IsValid { get; set; }
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        void HandleRequest();
    }
}