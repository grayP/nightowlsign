using System;
using System.Collections.Generic;

namespace nightowlsign.data.Models.Schedule
{
    public interface IScheduleManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        List<data.ScheduleAndSign> Get(data.Schedule Entity);
        data.Schedule Find(int ScheduleId);
        void UpdateDate(int id);
        bool Validate(data.Schedule entity);
        Boolean Update(data.Schedule entity);
        Boolean Insert(data.Schedule entity);
        bool Delete(data.Schedule entity);
    }
}