using System;
using System.Collections.Generic;
using System.Linq;
using nightowlsign.data;


namespace nightowlsign.data.Models.Schedule
{
    public class ScheduleManager
    {
        public ScheduleManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<data.Schedule> Get(data.Schedule Entity)
        {
            List<data.Schedule> ret = new List<data.Schedule>();
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.Schedules.OrderBy(x => x.Id).ToList<data.Schedule>();
            }

            if (!string.IsNullOrEmpty(Entity.Name))
            {
                ret = ret.FindAll(p => p.Name.ToLower().StartsWith(Entity.Name));
            }
            return ret;
        }

        public data.Schedule Find(int ScheduleId)
        {
            data.Schedule ret = null;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.Schedules.Find(ScheduleId);
            }
            return ret;

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
            if (Validate(entity))
            {
                try
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        db.Schedules.Attach(entity);
                        var modifiedStore = db.Entry(entity);
                        modifiedStore.Property(e => e.Name).IsModified = true;
                        modifiedStore.Property(e => e.StartDate).IsModified = true;
                        modifiedStore.Property(e => e.EndDate).IsModified = true;
                        modifiedStore.Property(e => e.Monday).IsModified = true;
                        modifiedStore.Property(e => e.Tuesday).IsModified = true;
                        modifiedStore.Property(e => e.Wednesday).IsModified = true;
                        modifiedStore.Property(e => e.Thursday).IsModified = true;
                        modifiedStore.Property(e => e.Friday).IsModified = true;
                        modifiedStore.Property(e => e.Saturday).IsModified = true;
                        modifiedStore.Property(e => e.Sunday).IsModified = true;
                        modifiedStore.Property(e => e.DefaultPlayList).IsModified = true;
                        modifiedStore.Property(e => e.StartTime).IsModified = true;
                        modifiedStore.Property(e => e.EndTime).IsModified = true;
                        modifiedStore.Property(e => e.Valid).IsModified = true;

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
            bool ret = false;
            try
            {
                ret = Validate(entity);
                if (ret)
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
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
                            Valid = entity.Valid
                        };

                        db.Schedules.Add(newSchedule);
                        db.SaveChanges();
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return ret;
            }
        }


        public bool Delete(data.Schedule entity)
        {
            bool ret = false;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                db.Schedules.Attach(entity);
                db.Schedules.Remove(entity);
                db.SaveChanges();
                ret = true;
            }
            return ret;
        }
    }

}
