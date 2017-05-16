using nightowlsign.data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace nightowlsign.data.Models.Schedule
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly Inightowlsign_Entities _context;
        public ScheduleManager(Inightowlsign_Entities context)
        {
            _context = context;
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<data.ScheduleAndSign> Get(data.Schedule entity)
        {
            var ret = _context.ScheduleAndSigns.OrderBy(x => x.Id).ToList<data.ScheduleAndSign>();
            if (entity.SignId > 0)
            {
                ret = ret.FindAll(p => p.SignId.Equals(entity.SignId));
            }
            if (!string.IsNullOrEmpty(entity.Name))
            {
                ret = ret.FindAll(p => p.Name.ToLower().StartsWith(entity.Name));
            }
            return ret;
        }

        public data.Schedule Find(int scheduleId)
        {
            return _context.Schedules.Find(scheduleId);
        }

        public void UpdateDate(int id)
        {
            var schedule = new data.Schedule() { Id = id, LastUpdated = DateTime.UtcNow.ToLocalTime() };
            _context.Schedules.Attach(schedule);
            _context.Entry(schedule).Property("LastUpdated").IsModified = true;
            _context.SaveChanges();
        }

        public bool Validate(data.Schedule entity)
        {
            ValidationErrors.Clear();
            if (!string.IsNullOrEmpty(entity.Name))
            {
                if (entity.Name.ToLower() == entity.Name)
                {
                    // ValidationErrors.Add(new KeyValuePair<string, string>("Schedule Name", "Schedule Name cannot be all lower case"));
                }
            }
            return (ValidationErrors.Count == 0);
        }


        public bool Update(data.Schedule entity)
        {
            DeleteLogs(entity.Id);
            return UpdateTheSchedule(entity);
        }
        private bool UpdateTheSchedule(data.Schedule entity)
        {
            entity.LastUpdated = DateTime.Now.ToUniversalTime();
            if (!Validate(entity)) return false;
            try
            {
                _context.Schedules.Attach(entity);
                var modifiedSchedule = _context.Entry(entity);
                modifiedSchedule.Property("Name").IsModified = true;
                modifiedSchedule.Property("StartDate").IsModified = true;
                modifiedSchedule.Property("EndDate").IsModified = true;
                modifiedSchedule.Property("Monday").IsModified = true;
                modifiedSchedule.Property("Tuesday").IsModified = true;
                modifiedSchedule.Property("Wednesday").IsModified = true;
                modifiedSchedule.Property("Thursday").IsModified = true;
                modifiedSchedule.Property("Friday").IsModified = true;
                modifiedSchedule.Property("Saturday").IsModified = true;
                modifiedSchedule.Property("Sunday").IsModified = true;
                modifiedSchedule.Property("DefaultPlayList").IsModified = true;
                modifiedSchedule.Property("StartTime").IsModified = true;
                modifiedSchedule.Property("EndTime").IsModified = true;
                modifiedSchedule.Property("Valid").IsModified = true;
                modifiedSchedule.Property("SignId").IsModified = true;
                modifiedSchedule.Property("LastUpdated").IsModified = true;

                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return false;
            }
        }

        private void DeleteLogs(int scheduleId)
        {
            try
            {
                var results = _context.UpLoadLogs.Where(u => u.ScheduleId == scheduleId);
                _context.UpLoadLogs.RemoveRange(results);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }


        public bool Insert(data.Schedule entity)
        {
            try
            {
                entity.LastUpdated = DateTime.Now.ToUniversalTime();
                if (!Validate(entity)) return true;
                var newSchedule = new data.Schedule()
                {
                    Name = entity.Name,
                    StartDate = entity.StartDate ?? DateTime.Now,
                    EndDate = entity.EndDate ?? DateTime.Now.AddMonths(3),
                    Monday = entity.Monday,
                    Tuesday = entity.Tuesday,
                    Wednesday = entity.Wednesday,
                    Thursday = entity.Thursday,
                    Friday = entity.Friday,
                    Saturday = entity.Saturday,
                    Sunday = entity.Sunday,
                    DefaultPlayList = entity.DefaultPlayList,
                    StartTime = entity.StartTime,
                    EndTime = entity.EndTime,
                    Valid = entity.Valid,
                    SignId = entity.SignId,
                    LastUpdated = entity.LastUpdated
                };
                _context.Schedules.Add(newSchedule);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return false;
            }
        }

        public bool Delete(data.Schedule entity)
        {
            DeleteSchedules(entity);
            DeleteScheduleImages(entity.Id);
            DeleteScheduleStore(entity.Id);
            return true;
        }

        private void DeleteSchedules(data.Schedule entity)
        {
            _context.Schedules.Attach(entity);
            _context.Schedules.Remove(entity);
            _context.SaveChanges();
        }


        private void DeleteScheduleImages(int scheduleId)
        {
            try
            {
                var range = _context.ScheduleImages.Where(l => l.ScheduleID == scheduleId);
                _context.ScheduleImages.RemoveRange(range);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void DeleteScheduleStore(int scheduleId)
        {
            try
            {
                var range = _context.ScheduleStores.Where(l => l.ScheduleID == scheduleId);
                _context.ScheduleStores.RemoveRange(range);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

}
