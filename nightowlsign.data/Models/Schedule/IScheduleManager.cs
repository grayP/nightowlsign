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
        bool Update(data.Schedule entity);
        bool Insert(data.Schedule entity);
        bool Delete(data.Schedule entity);
        void Copy(data.Schedule entity);
    }
}