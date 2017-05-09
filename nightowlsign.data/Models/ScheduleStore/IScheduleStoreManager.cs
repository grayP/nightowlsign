using System.Collections.Generic;

namespace nightowlsign.data.Models.ScheduleStore
{
    public interface IScheduleStoreManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        List<int?> Get(data.Schedule entity);
        List<StoreSelect> GetAllStores(int signSizeId);
        Store Find(int id);
        void UpdateStoreList(StoreSelect storeSelect, data.Schedule schedule);
        void Delete(int v);
        data.ScheduleStore GetValues(StoreSelect storeSelect);

    }
}