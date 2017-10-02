using Newtonsoft.Json;
using nightowlsign.data.Interfaces;
using nightowlsign.data.Models.Logging;
using SignSystemAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace nightowlsign.data.Models.Stores
{
    public class StoreManager : IStoreManager
    {
        private readonly Inightowlsign_Entities _context;
        private readonly ILoggingManager _loggingManager;
        private data.Schedule defaultSchedule;
        private Sign defaultSign;
        public StoreManager(Inightowlsign_Entities context , ILoggingManager loggingManager)
        {
            _context = context;
            _loggingManager = loggingManager;
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

        public async Task<List<StoreAndSign>> GetAsync(Store entity)
        {
            var client = ApiClient.GetClient();
            HttpResponseMessage response = await client.GetAsync("api/store");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IEnumerable<StoreAndSign>>(content);
                return model.ToList();
            }
            else
            {
                return null;
            }
        }

        public StoreAndSign GetOneStore(int id)
        {                       
            return _context.StoreAndSigns.FirstOrDefault(i => i.id == id);
        }
        public List<StoreAndSign> Get(Store entity)
        {
            var ret = _context.StoreAndSigns.OrderBy(x => x.Name).ToList<StoreAndSign>();
            if (!string.IsNullOrEmpty(entity.Name))
            {
                ret = ret.FindAll(p => p.Name.ToLower().StartsWith(entity.Name));
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
                //store.AvailableSchedules = GetAvailableSchedules(store.id);
                //store.SelectedSchedules = GetSelectedSchedules(store.id);
                store.DefaultSchedule = GetDefaultSchedule(store.id) ?? defaultSched;
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

        private data.Schedule GetDefaultSchedule(int storeId)
        {
            var ret = (from s in _context.ScheduleStores
                       join sc in _context.Schedules on s.ScheduleID equals sc.Id
                       where s.StoreId == storeId & sc.DefaultPlayList == true
                       select new { sc.Id, sc.Name })
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

        public bool Update(Store entity)
        {
            entity.LastUpdateTime = DateTime.Now;
            if (!Validate(entity)) return false;
            try
            {
                using (var db = new nightowlsign_Entities())
                {
                    db.Store.Attach(entity);
                    var modifiedStore = db.Entry(entity);
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
                    modifiedStore.Property(e => e.NumImages).IsModified = true;
                    modifiedStore.Property(e => e.LastUpdateStatus).IsModified = true;
                    db.SaveChanges();
                    _loggingManager.Insert("Update of properties", entity.Name);
                    return true;
                }
            }
            catch (Exception ex)
            {
               return !_loggingManager.Insert(ex);
                
            }
        }

        public bool Insert(Store entity)
        {
            try
            {
                if (!Validate(entity)) return false;
                var newStore = new Store()
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
                    ProgramFile = entity.ProgramFile,
                    NumImages = entity.NumImages
                };

                _context.Store.Add(newStore);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return !_loggingManager.Insert(ex);
            }
        }

        public bool Delete(Store entity)
        {
            _context.Store.Attach(entity);
            _context.Store.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public bool ResetLastStatus(Store entity)
        {
                entity.LastUpdateStatus = -99;
                _loggingManager.Insert("Reset completed", entity.Name);
                return Update(entity);
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
