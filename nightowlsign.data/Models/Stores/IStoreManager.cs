using System.Collections.Generic;
using System.Threading.Tasks;

namespace nightowlsign.data.Models.Stores
{
    public interface IStoreManager
    {
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        Task <List<StoreAndSign>> GetAsync(Store entity);
        List<StoreAndSign> Get(Store entity);
        Store Find(int storeId);
        bool Validate(Store entity);
        bool Update(Store entity);
        bool Insert(Store entity);
        bool Delete(Store entity);
    }
}