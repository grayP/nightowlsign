using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using nightowlsign.data.Interfaces;


namespace nightowlsign.data.Models.StoreScheduleLog
{
    public class StoreScheduleLogManager : IStoreScheduleLogManager
    {
        public string ErrorMessage { get; set; }
        public data.StoreScheduleLog Entity { get; set; }
        private readonly Inightowlsign_Entities _context;

        public StoreScheduleLogManager(Inightowlsign_Entities context)
        {
            _context = context;
        }

        public List<data.StoreScheduleLog> Get(data.StoreScheduleLog storeScheduleLog)
        {
            return _context.StoreScheduleLogs.OrderBy(x => x.ScheduleName).Where(p => p.ScheduleName.ToLower().StartsWith(storeScheduleLog.ScheduleName)).ToList<data.StoreScheduleLog>();
        }

        private data.StoreScheduleLog GetStoreScheduleLog(int scheduleStoreLogId)
        {

            return _context.StoreScheduleLogs.Find(scheduleStoreLogId);

        }

        public bool Insert()
        {
            try
            {

                var newStoreScheduleLog = new data.StoreScheduleLog()
                {
                    ScheduleName = Entity.ScheduleName.Trim(),
                    DateInstalled = DateTime.Now,
                    StoreId = Entity.StoreId,
                    InstalledOk = true,
                    ScheduleId = Entity.ScheduleId
                };
                _context.StoreScheduleLogs.Add(newStoreScheduleLog);
                _context.SaveChanges();

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

            _context.StoreScheduleLogs.Attach(entity);
            _context.StoreScheduleLogs.Remove(entity);
            _context.SaveChanges();
            return true;

        }

        public void DeleteLog(int scheduleId)
        {
            try
            {
                _context.StoreScheduleLogs.RemoveRange(_context.StoreScheduleLogs.Where(x => x.ScheduleId == scheduleId));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}

