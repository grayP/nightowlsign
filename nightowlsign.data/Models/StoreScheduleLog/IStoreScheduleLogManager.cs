using System.Collections.Generic;

namespace nightowlsign.data.Models.StoreScheduleLog
{
    public interface IStoreScheduleLogManager
    {
        string ErrorMessage { get; set; }
        data.StoreScheduleLog Entity { get; set; }
        List<data.StoreScheduleLog> Get(data.StoreScheduleLog storeScheduleLog);
        bool Insert();
        bool Delete(data.StoreScheduleLog entity);
        void DeleteLog(int v);
    }
}