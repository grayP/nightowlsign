using System.Collections.Generic;

namespace nightowlsign.data.Models.Stores
{
    public interface IStoreManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        List<StoreAndSign> Get(Store entity);
        Store Find(int storeId);
        bool Validate(Store entity);
        bool Update(Store entity);
        bool Insert(Store entity);
        bool Delete(Store entity);
    }
}