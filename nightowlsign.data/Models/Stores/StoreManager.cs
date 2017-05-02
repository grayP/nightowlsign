using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using nightowlsign.data.Interfaces;


namespace nightowlsign.data.Models.Stores
{
    public class StoreManager
    {
        private nightowlsign_Entities _context;
        private data.Schedule defaultSchedule;
        private Sign defaultSign;
        public StoreManager(nightowlsign_Entities context)
        {
            _context = context;
            ValidationErrors = new List<KeyValuePair<string, string>>();
            defaultSchedule = new data.Schedule
            {
                Name = "No default playlist",
                StartDate = null,
                EndDate = null,
                Id = 0,
                SignId = 0
            };
            defaultSign = new Sign
            {
                Model = "None",
                Width = 0,
                Height = 1,
                id = 0,
                StoreId = 0,
                InstallDate = DateTime.Now

            };
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<StoreAndSign> Get(Store Entity)
        {
            var ret = _context.StoreAndSigns.OrderBy(x => x.Name).ToList<StoreAndSign>();
            if (!string.IsNullOrEmpty(Entity.Name))
            {
                ret = ret.FindAll(p => p.Name.ToLower().StartsWith(Entity.Name));
            }
            GetPlayLists(ret);
            return ret;
        }

        private void GetPlayLists(List<StoreAndSign> storeList)
        {
            foreach (var store in storeList)
            {
                data.Schedule defaultSched =
                new data.Schedule { Id = 0, Name = "" };
                store.AvailableSchedules = GetAvailableSchedules(store.id);
                store.SelectedSchedules = GetSelectedSchedules(store.id);
                store.DefaultSchedule = GetDefaultSchedule(store.SignId ?? 0) ?? defaultSched;
                store.CurrentSchedule = GetInstalledSchedule(store.id) ?? defaultSched;
                store.Sign = GetSign(store.SignId ?? 0) ?? defaultSign;
            }
        }

        private Sign GetSign(int signId)
        {
            return _context.Signs.Find(signId);
        }

        private List<data.Schedule> GetSelectedSchedules(int storeId)
        {
            var ret = (from s in _context.Schedules
                       join st in _context.ScheduleStores on s.Id equals st.ScheduleID
                       where st.StoreId == storeId
                       select new { s.Id, s.Name, s.StartDate, s.EndDate, s.DefaultPlayList, s.StartTime, s.EndTime })
                .AsEnumerable()
                .Select(x => new data.Schedule()
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DefaultPlayList = x.DefaultPlayList,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime
                });
            return ret.ToList();
        }

        private data.Schedule GetDefaultSchedule(int signId)
        {
            var ret = (from s in _context.Schedules
                       where s.SignId == signId && s.DefaultPlayList == true
                       select new { s.Id, s.Name })
                .AsEnumerable()
                .OrderByDescending(x => x.Id)
                .Select(x => new data.Schedule()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).FirstOrDefault();

            return ret;
        }

        private data.Schedule GetInstalledSchedule(int storeId)
        {
            var ret = (from s in _context.StoreScheduleLogs
                       where s.StoreId == storeId
                       select new { s.ScheduleId, s.ScheduleName, s.DateInstalled })
                .AsEnumerable()
                .OrderByDescending(x => x.DateInstalled)
                .Select(x => new data.Schedule()
                {
                    Id = x.ScheduleId ?? 0,
                    Name = x.ScheduleName,
                }).FirstOrDefault();

            return ret;
        }

        private List<data.Schedule> GetAvailableSchedules(int storeId)
        {
            var ret = (from s in _context.Schedules
                       join st in _context.Store on s.SignId equals st.SignId
                       where st.id == storeId
                       select new { s.Id, s.Name, s.StartDate, s.EndDate, s.DefaultPlayList, s.StartTime, s.EndTime })
               .AsEnumerable()
               .Select(x => new data.Schedule()
               {
                   Id = x.Id,
                   Name = x.Name,
                   StartDate = x.StartDate,
                   EndDate = x.EndDate,
                   DefaultPlayList = x.DefaultPlayList,
                   StartTime = x.StartTime,
                   EndTime = x.EndTime
               });
            return ret.ToList();
        }

        public Store Find(int storeId)
        {
            Store ret = null;
            return _context.Store.Find(storeId);

        }

        public bool Validate(Store entity)
        {
            ValidationErrors.Clear();
            if (!string.IsNullOrEmpty(entity.Name))
            {
                if (entity.Name.ToLower() == entity.Name)
                {
                    ValidationErrors.Add(new KeyValuePair<string, string>("Store Name", "Store Name cannot be all lower case"));
                }
            }
            return (ValidationErrors.Count == 0);
        }


        public Boolean Update(Store entity)
        {
            bool ret = false;
            if (Validate(entity))
            {
                try
                {
                    _context.Store.Attach(entity);
                    var modifiedStore = _context.Entry(entity);
                    modifiedStore.Property(e => e.Name).IsModified = true;
                    modifiedStore.Property(e => e.Address).IsModified = true;
                    modifiedStore.Property(e => e.Suburb).IsModified = true;
                    modifiedStore.Property(e => e.State).IsModified = true;
                    modifiedStore.Property(e => e.Manager).IsModified = true;
                    modifiedStore.Property(e => e.Phone).IsModified = true;
                    modifiedStore.Property(e => e.SignId).IsModified = true;
                    modifiedStore.Property(e => e.IpAddress).IsModified = true;
                    modifiedStore.Property(e => e.SubMask).IsModified = true;
                    modifiedStore.Property(e => e.Port).IsModified = true;
                    modifiedStore.Property(e => e.ProgramFile).IsModified = true;
                    _context.SaveChanges();
                    ret = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    ret = false;
                }
            }
            return ret;
        }

        public Boolean Insert(Store entity)
        {
            bool ret = false;
            try
            {
                ret = Validate(entity);
                if (ret)
                {
                        Store newStore = new Store()
                        {
                            Name = entity.Name.Trim(),
                            Address = entity.Address,
                            Suburb = entity.Suburb,
                            State = entity.State,
                            Manager = entity.Manager,
                            Phone = entity.Phone,
                            SignId = entity.SignId,
                            IpAddress = entity.IpAddress,
                            SubMask = entity.SubMask,
                            Port = entity.Port,
                            ProgramFile = entity.ProgramFile
                        };

                        _context.Store.Add(newStore);
                        _context.SaveChanges();
                        ret = true;
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return ret;
            }
        }


        public bool Delete(Store entity)
        {
            bool ret = false;
            _context.Store.Attach(entity);
            _context.Store.Remove(entity);
            _context.SaveChanges();
            ret = true;
            return ret;
        }
    }

    public class SelectPlayList
    {
        public int Id { get; set; }
        public string PlayListName { get; set; }
        public int? StoreId { get; set; }
        public string URL { get; set; }
    }
}
