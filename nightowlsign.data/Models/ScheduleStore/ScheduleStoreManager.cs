using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using nightowlsign.data;
using nightowlsign.data.Interfaces;


namespace nightowlsign.data.Models.ScheduleStore
{
    public class ScheduleStoreManager : IScheduleStoreManager
    {
        private readonly Inightowlsign_Entities _context;
        public ScheduleStoreManager(Inightowlsign_Entities context)
        {
            _context = context;
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<int?> Get(data.Schedule entity)
        {
            var query = (from c in _context.ScheduleStores
                         where c.ScheduleID == entity.Id
                         select c.StoreId);
            return query.ToList();
        }

        public List<StoreSelect> GetAllStores(int signSizeId)
        {
            var query = (from s in _context.Store.Where(e => e.SignId == signSizeId)
                         select new StoreSelect { StoreId = s.id, Name = s.Name, SignId = s.SignId ?? 0 });
            return query.ToList();

        }

        public Store Find(int id)
        {
            return _context.Store.Find(id);
        }

        public void UpdateStoreList(StoreSelect storeSelect, data.Schedule schedule)
        {

            var scheduleStore = _context.ScheduleStores.Find(storeSelect.Id);
            if (storeSelect.Selected)
            {
                if (scheduleStore != null) return;
                var storeSelected = new data.ScheduleStore()
                {
                    StoreId = storeSelect.StoreId,
                    ScheduleID = schedule.Id,
                    Id = storeSelect.Id
                };
                _context.ScheduleStores.Add(storeSelected);
            }
            else
            {
                if (scheduleStore == null) return;
                _context.ScheduleStores.Attach(scheduleStore);
                _context.ScheduleStores.Remove(scheduleStore);
            }
            _context.SaveChanges();

        }

        public data.ScheduleStore GetValues(StoreSelect storeSelect)
        {
            return _context.ScheduleStores.FirstOrDefault(x => x.StoreId == storeSelect.StoreId && x.ScheduleID == storeSelect.ScheduleId);
        }

        public bool IsSelected(int scheduleId, int storeId)
        {
            return _context.ScheduleStores.Any(x => x.StoreId == storeId && x.ScheduleID == scheduleId);
        }

        public void Delete(int scheduleId)
        {
            try
            {
                _context.ScheduleStores.RemoveRange(_context.ScheduleStores.Where(x => x.ScheduleID == scheduleId));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
