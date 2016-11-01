using nightowlsign.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightowlsign.data.Models
{
    public class StoreSignViewModel : BaseModel.ViewModelBase
    {
        public StoreSignViewModel(int id, string storeName) : base()
        {
            store = new Store
            {
                id = id,
                Name = storeName
            };
            AllSigns = new List<SignSelect>();
        }


        public Store store { get; set; }
        public List<SignSelect> AllSigns { get; set; }
        public List<int?> storesign { get; set; }

        protected override void Init()
        {

            base.Init();
        }

        public override void HandleRequest()
        {
            base.HandleRequest();
        }

        public void loadData()
        {
            Get();
        }

        protected override void Get()
        {
            StoreSignManager sm = new StoreSignManager();
            storesign = sm.Get(store);
            AllSigns = sm.GetAllSigns(store.id);
            foreach (SignSelect ss in AllSigns )
            {
               StoreSign selected=sm.GetValues(ss);
                if (selected != null)
                {
                    ss.selected = true;
                    ss.Id = selected.id;
                    ss.IpAddress = selected.IPAddress;
                    ss.SubMask = selected.SubMask;
                    ss.Port = selected.Port;
                }
            }
        }

    }
}
