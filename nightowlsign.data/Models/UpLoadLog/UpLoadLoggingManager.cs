using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightowlsign.data.Interfaces;

namespace nightowlsign.data.Models.UpLoadLog
{
    public interface IUpLoadLoggingManager
    {
        List<data.UpLoadLog> GetLatest();
        bool UpLoadNeeded(int storeId, string filename, DateTime lastUpdated);
        void Delete(int scheduleId);
        int? GetOverallStatus(int storeId, DateTime? lastUpdated, int scheduleid);
        bool Upsert(data.UpLoadLog log);
    }

    public class UpLoadLoggingManager : IUpLoadLoggingManager
    {
        private readonly Inightowlsign_Entities _context;
        public UpLoadLoggingManager(Inightowlsign_Entities context)
        {
            _context = context;
        }

        public List<data.UpLoadLog> GetLatest()
        {
            return _context.UpLoadLogs.OrderByDescending(i => i.DateTime).Take(50).ToList();
        }

        public bool UpLoadNeeded(int storeId, string filename, DateTime lastUpdated)
        {
            var upLoadLog = _context.UpLoadLogs.Where(i => i.StoreId == storeId && i.ProgramFile == filename)
                .OrderByDescending(i => i.DateTime).FirstOrDefault();
            if (upLoadLog == null) return true;
            return upLoadLog.ResultCode != 0 || !(upLoadLog.DateTime > lastUpdated);
        }

        public void Delete(int scheduleId)
        {
            var results = _context.UpLoadLogs.Where(u => u.ScheduleId == scheduleId);
            _context.UpLoadLogs.RemoveRange(results);
            _context.SaveChanges();
        }
        public int? GetOverallStatus(int storeId, DateTime? lastUpdated, int scheduleid)
        {
            return _context.UpLoadLogs.Where(i => i.StoreId == storeId && i.ScheduleId == scheduleid).Sum(i => i.ResultCode) ?? -99;
        }

        public bool Upsert(data.UpLoadLog log)
        {
            try
            {
                var existingLog = _context.UpLoadLogs
                    .Find(log.ProgramFile, log.StoreId, log.ScheduleId);

                if (existingLog != null)
                {
                    _context.Entry(existingLog).CurrentValues.SetValues(log);
                }
                else
                {
                    var newLog = new data.UpLoadLog()
                    {
                        StoreId = log.StoreId,
                        ResultCode = log.ResultCode,
                        ProgramFile = log.ProgramFile,
                        DateTime = log.DateTime,
                        ScheduleId = log.ScheduleId
                    };
                    _context.UpLoadLogs.Add(newLog);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return false;
            }
        }
    }
}
