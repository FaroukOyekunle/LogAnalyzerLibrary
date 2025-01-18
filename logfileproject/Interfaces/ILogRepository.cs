using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using logfileproject.Models;

namespace logfileproject.Interfaces
{
    public interface ILogRepository
    {
        IEnumerable<LogEntry> GetLogs(string directory, DateTime? startDate, DateTime? endDate);
        void ArchiveLogs(string directory, DateTime startDate, DateTime endDate);
        void DeleteLogs(string directory, DateTime startDate, DateTime endDate);
        IEnumerable<LogEntry> SearchLogsBySize(string directory, long minSize, long maxSize);
        IEnumerable<string> GetDirectories(string baseDirectory);
    }
}