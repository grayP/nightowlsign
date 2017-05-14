﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightowlsign.data.Interfaces;
using nightowlsign.data.Models.ScheduleStore;
using nightowlsign.data.Models.StoreScheduleLog;
using nightowlsign.data.Models.UpLoadLog;

namespace nightowlsign.data.Models.Schedule
{
    public class ScheduleViewModel : BaseModel.ViewModelBase, IScheduleViewModel
    {
        private readonly Inightowlsign_Entities _context;
        private readonly IScheduleManager _scheduleManager;
        private readonly IScheduleStoreManager _scheduleStoreManager;
        private readonly IStoreScheduleLogManager _storeScheduleLogManager;
        private readonly IUpLoadLoggingManager _upLoadLoggingManager;
       // public ScheduleViewModel(Inightowlsign_Entities context, IScheduleManager scheduleManager, IScheduleStoreManager scheduleStoreManager, IStoreScheduleLogManager storeScheduleLogManager)
        public ScheduleViewModel(Inightowlsign_Entities context)
        {
             _context = context;
            // _scheduleManager = scheduleManager;
            // _scheduleStoreManager = scheduleStoreManager;
            // _storeScheduleLogManager = storeScheduleLogManager;
           // _context = new nightowlsign_Entities();
            _scheduleManager = new ScheduleManager(_context);
            _scheduleStoreManager = new ScheduleStoreManager(_context);
            _storeScheduleLogManager = new StoreScheduleLogManager(_context);
            _upLoadLoggingManager= new UpLoadLoggingManager(_context);
        }


        public List<data.ScheduleAndSign> Schedules { get; set; }
        public data.Schedule SearchEntity { get; set; }
        public data.Schedule Entity { get; set; }
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
            Schedules = new List<data.ScheduleAndSign>();
            SearchEntity = new data.Schedule();
            Entity = new data.Schedule();
            base.Init();
        }
        public override void HandleRequest()
        {
            base.HandleRequest();
        }
        protected override void ResetSearch()
        {
            SearchEntity = new data.Schedule();
        }

        protected override void Get()
        {
            Schedules = _scheduleManager.Get(SearchEntity);
        }

        protected override void Edit()
        {
            Entity = _scheduleManager.Find(Convert.ToInt32(EventArgument));
            base.Edit();
        }

        protected override void Add()
        {
            IsValid = true;
            Entity = new data.Schedule();
            base.Add();
        }
        protected override void Save()
        {
            if (Mode == "Add")
            {
                _scheduleManager.Insert(Entity);
            }
            else
            {
                _scheduleManager.Update(Entity);
                 _upLoadLoggingManager.Delete(Entity.Id);
            }
            ValidationErrors = _scheduleManager.ValidationErrors;
            base.Save();
        }

        protected override void Delete()
        {
            Entity = _scheduleManager.Find(Convert.ToInt32(EventArgument));
            _scheduleManager.Delete(Entity);
            _scheduleStoreManager.Delete(Convert.ToInt32(EventArgument));
            _storeScheduleLogManager.DeleteLog(Convert.ToInt32(EventArgument));
            Get();
            base.Delete();
        }
    }
}
