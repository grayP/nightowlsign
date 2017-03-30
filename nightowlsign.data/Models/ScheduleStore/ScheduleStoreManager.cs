using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using nightowlsign.data;


namespace nightowlsign.data.Models.ScheduleStore
{
    public class ScheduleStoreManager
    {
        public ScheduleStoreManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<int?> Get(data.Schedule Entity)
        {
            int scheduleID = Entity.Id;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                var query = (from c in db.ScheduleStores
                             where c.ScheduleID == scheduleID
                             select c.StoreId);
                return query.ToList();
            }

        }

        public List<StoreSelect> GetAllStores(int SignSizeId)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                var query = (from s in db.Store.Where(e=>e.SignId==SignSizeId)
                             select new StoreSelect { StoreId= s.id, Name = s.Name, SignId = s.SignId ?? 0});
                return query.ToList();
            }
        }


        public Store Find(int id)
        {
            Store ret = null;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.Store.Find(id);
            }
            return ret;

        }

        public void UpdateStoreList(StoreSelect storeSelect, data.Schedule schedule)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                data.ScheduleStore storeSelected = new data.ScheduleStore()
                {
                    StoreId = storeSelect.StoreId,
                    ScheduleID = schedule.Id,
                    Id=storeSelect.Id
               };
                if (storeSelect.Selected)
                {
                    db.Set<data.ScheduleStore>().AddOrUpdate(storeSelected);
                    db.SaveChanges();
                }
                else
                {
                    data.ScheduleStore scheduleStore =
                        db.ScheduleStores.Find(storeSelect.Id);
                    if (scheduleStore!=null)
                    {
                        db.ScheduleStores.Attach(scheduleStore);
                        db.ScheduleStores.Remove(scheduleStore);
                        db.SaveChanges();
                    }
                }
            }
        }

        internal data.ScheduleStore GetValues(StoreSelect storeSelect)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                return db.ScheduleStores.FirstOrDefault(x => x.StoreId == storeSelect.StoreId && x.ScheduleID == storeSelect.ScheduleId);
            }
        }

        internal bool IsSelected(int ScheduleId, int storeId)
        {
            bool ret = false;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.ScheduleStores.Any(x => x.StoreId == storeId && x.ScheduleID == ScheduleId);
            }
            return ret;
        }

        internal void Delete(int scheduleId)
        {
            try
            {
                using (nightowlsign_Entities db = new nightowlsign_Entities())
                {
                    db.ScheduleStores.RemoveRange(db.ScheduleStores.Where(x => x.ScheduleID == scheduleId));
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
