using System;
using System.Collections.Generic;

namespace nightowlsign.data.Models.Logging
{
    public interface ILoggingManager
    {
        List<data.Logging> GetLatest(string filter);
        bool Insert(data.Logging log);
        bool Insert(Exception ex);
        bool Insert(string desc, string sub);
    }
}