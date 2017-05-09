﻿using nightowlsign.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightowlsign.data.Interfaces;
using nightowlsign.data.Models.ScheduleStore;

namespace nightowlsign.data.Models.Stores
{
    public class StoreViewModel : BaseModel.ViewModelBase, IStoreViewModel
    {
        private readonly Inightowlsign_Entities _context;
        private readonly IStoreManager _storeManager;
        public StoreViewModel(Inightowlsign_Entities context, IStoreManager  storeManager) : base()
        {
            _context = context;
            _storeManager = storeManager;
        }
        public List<Store> Stores { get; set; }
        public List<StoreAndSign> StoresAndSigns { get; set; }
        public Store SearchEntity { get; set; }
        public Store Entity { get; set; }
        public IEnumerable<SelectListItem> SignList
        {
            get
            {
                var selectList = new List<SelectListItem>()
                    {
                        new SelectListItem
                        {
                            Id = 0,
                            Model = "Show All"
                        }
                    };
                selectList.AddRange(from item in
                                  _context.Signs.OrderBy(x => x.Model)
                                    select new SelectListItem()
                                    {
                                        SignId = item.id,
                                        Model = item.Model
                                    });

                return selectList;
            }
        }

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
            StoresAndSigns = _storeManager.Get(SearchEntity);

        }
        protected override void Edit()
        {
            Entity = _storeManager.Find(Convert.ToInt32(EventArgument));
            base.Edit();
        }
        protected override void Add()
        {
            IsValid = true;
            Entity = new Store {Name = ""};
            base.Add();
        }
        protected override void Save()
        {
            
            if (Mode == "Add")
            {
                _storeManager.Insert(Entity);
            }
            else
            {
                _storeManager.Update(Entity);
            }
            ValidationErrors = _storeManager.ValidationErrors;
            base.Save();
        }
        protected override void Delete()
        {
            Entity = _storeManager.Find(Convert.ToInt32(EventArgument));
            _storeManager.Delete(Entity);
            Get();
            base.Delete();
        }
    }
}
