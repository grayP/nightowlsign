using nightowlsign.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightowlsign.data.Models.Stores
{
    public class StoreViewModel : BaseModel.ViewModelBase
    {
        public StoreViewModel() : base()
        {
        }
        public List<Store> Stores { get; set; }
        public List<LastInstalledSchedule> StoreSchedules { get; set; }
        public Store SearchEntity { get; set; }
        public Store Entity { get; set; }

        protected override void Init()
        {
            Stores = new List<Store>();
            SearchEntity = new Store();
            Entity = new Store();
            base.Init();
        }
        public override void HandleRequest()
        {
            base.HandleRequest();
        }
        protected override void ResetSearch()
        {
            SearchEntity = new Store();
        }
        protected override void Get()
        {
            StoreManager sm = new StoreManager();
            StoreSchedules = sm.Get(SearchEntity);

        }
        protected override void Edit()
        {
            StoreManager sm = new StoreManager();
            Entity = sm.Find(Convert.ToInt32(EventArgument));
            base.Edit();
        }
        protected override void Add()
        {
            IsValid = true;
            Entity = new Store();
            Entity.Name = "";
            base.Add();
        }
        protected override void Save()
        {
            StoreManager sm = new StoreManager();
            if (Mode == "Add")
            {
                sm.Insert(Entity);
            }
            else
            {
                sm.Update(Entity);
            }
            ValidationErrors = sm.ValidationErrors;
            base.Save();
        }
        protected override void Delete()
        {
            StoreManager sm = new StoreManager();
            Entity = sm.Find(Convert.ToInt32(EventArgument));
            sm.Delete(Entity);
            Get();
            base.Delete();
        }
    }
}
