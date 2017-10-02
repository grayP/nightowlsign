using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightowlsign.data.Interfaces;

namespace nightowlsign.data.Models.Logging
{
    public class LoggingManager : ILoggingManager
    {
        private readonly Inightowlsign_Entities _context;
        public LoggingManager(Inightowlsign_Entities context)
        {
            _context = context;
        }

        public List<data.Logging> GetLatest(string filter)
        {
            return string.IsNullOrEmpty(filter) ? _context.Loggings.OrderByDescending(i => i.DateStamp).Take(50).ToList() : _context.Loggings.Where(i=>i.Subject==filter).OrderByDescending(i => i.DateStamp).Take(50).ToList();
        }
        public bool Insert( string description, string Subject)
        {
            data.Logging log = new data.Logging();
            log.DateStamp = DateTime.Now;
            log.Description = description;
            log.Subject = Subject;
            return Insert(log);

        }

        public bool Insert(Exception ex)
        {
            data.Logging log = new data.Logging();
            log.DateStamp = DateTime.Now;
            log.Description = ex.Message;
            string subject = ex.InnerException.ToString();
            log.Subject = subject.Substring(0, Math.Min(subject.Length, 25));
            return Insert(log);
        }

        public bool Insert(data.Logging log)
        {
            try
            {
                var newLog = new data.Logging()
                {
                    Description = log.Description.Trim(),
                    Subject = log.Subject.Trim(),
                    DateStamp =DateTime.Now.ToLocalTime()
                 
                };

                _context.Loggings.Add(newLog);
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
