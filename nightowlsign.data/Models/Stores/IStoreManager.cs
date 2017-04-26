using System;
using System.Collections.Generic;

namespace nightowlsign.data.Models.Stores
{
    public interface IStoreManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        List<StoreAndSign> Get(Store Entity);
        void GetPlayLists(List<StoreAndSign> storeList);
        Sign GetSign(int signId);
        List<data.Schedule> GetSelectedSchedules(int storeId);
        data.Schedule GetDefaultSchedule(int signId);
        data.Schedule GetInstalledSchedule(int storeId);
        List<data.Schedule> GetAvailableSchedules(int storeId);
        Store Find(int storeId);
        bool Validate(Store entity);
        Boolean Update(Store entity);
        Boolean Insert(Store entity);
        bool Delete(Store entity);
    }
}