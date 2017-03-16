﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace nightowlsign.data.Models.StoreScheduleLog
{
    public class StoreScheduleLogManager
    {
        public string ErrorMessage { get; set; }
        public data.StoreScheduleLog Entity { get; set; }



        public List<data.StoreScheduleLog> Get(data.StoreScheduleLog storeScheduleLog)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                var ret = db.StoreScheduleLogs.OrderBy(x => x.ScheduleName).ToList<data.StoreScheduleLog>();
                ret = ret.FindAll(p => p.ScheduleName.ToLower().StartsWith(storeScheduleLog.ScheduleName));
                return ret;
            }
        }


        private data.StoreScheduleLog GetStoreScheduleLog(int scheduleStoreLogId)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                return db.StoreScheduleLogs.Find(scheduleStoreLogId);
            }
        }

        public bool Insert()
        {
            try
            {
                using (nightowlsign_Entities db = new nightowlsign_Entities())
                {
                    data.StoreScheduleLog newStoreScheduleLog = new data.StoreScheduleLog()
                    {
                        ScheduleName = Entity.ScheduleName.Trim(),
                        DateInstalled = DateTime.Now,
                        StoreId = Entity.StoreId,
                        InstalledOk = true,
                        ScheduleId = Entity.ScheduleId
                    };
                    db.StoreScheduleLogs.Add(newStoreScheduleLog);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }


        public bool Delete(data.StoreScheduleLog entity)
        {
            bool ret = false;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                db.StoreScheduleLogs.Attach(entity);
                db.StoreScheduleLogs.Remove(entity);
                db.SaveChanges();
                ret = true;
            }
            return ret;
        }

        internal void DeleteLog(int scheduleId)
        {
            try
            {
                using (nightowlsign_Entities db = new nightowlsign_Entities())
                {
                    db.StoreScheduleLogs.RemoveRange(db.StoreScheduleLogs.Where(x => x.ScheduleId == scheduleId));
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

