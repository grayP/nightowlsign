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
            // List<data.ScheduleAndSign> ret = new List<data.ScheduleAndSign>();
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
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                db.Schedules.Attach(schedule);
                db.Entry(schedule).Property(e => e.LastUpdated).IsModified = true;
                db.SaveChanges();
            }
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


        public Boolean Update(data.Schedule entity)
        {
            bool ret = false;
            entity.LastUpdated = DateTime.Now.ToLocalTime();
            if (Validate(entity))
            {
                try
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        db.Schedules.Attach(entity);
                        var modifiedSchedule = db.Entry(entity);
                        modifiedSchedule.Property(e => e.Name).IsModified = true;
                        modifiedSchedule.Property(e => e.StartDate).IsModified = true;
                        modifiedSchedule.Property(e => e.EndDate).IsModified = true;
                        modifiedSchedule.Property(e => e.Monday).IsModified = true;
                        modifiedSchedule.Property(e => e.Tuesday).IsModified = true;
                        modifiedSchedule.Property(e => e.Wednesday).IsModified = true;
                        modifiedSchedule.Property(e => e.Thursday).IsModified = true;
                        modifiedSchedule.Property(e => e.Friday).IsModified = true;
                        modifiedSchedule.Property(e => e.Saturday).IsModified = true;
                        modifiedSchedule.Property(e => e.Sunday).IsModified = true;
                        modifiedSchedule.Property(e => e.DefaultPlayList).IsModified = true;
                        modifiedSchedule.Property(e => e.StartTime).IsModified = true;
                        modifiedSchedule.Property(e => e.EndTime).IsModified = true;
                        modifiedSchedule.Property(e => e.Valid).IsModified = true;
                        modifiedSchedule.Property(e => e.SignId).IsModified = true;
                        modifiedSchedule.Property(e => e.LastUpdated).IsModified = true;

                        db.SaveChanges();
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    ret = false;
                }
            }
            return ret;
        }

        public Boolean Insert(data.Schedule entity)
        {
            try
            {
                entity.LastUpdated = DateTime.Now.ToLocalTime();
                if (Validate(entity))
                {
                    data.Schedule newSchedule = new data.Schedule()
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
                }
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
            _context.Schedules.Attach(entity);
            _context.Schedules.Remove(entity);
            _context.SaveChanges();
            return true;

        }
    }

}
