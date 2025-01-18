using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using logfileproject.Models;

namespace logfileproject.Interfaces
{
    public interface ILogService
    {
        int CountUniqueErrors(string directory);
        int CountDuplicatedErrors(string directory);
        void ArchiveLogs(string directory, DateTime startDate, DateTime endDate);
        void DeleteLogs(string directory, DateTime startDate, DateTime endDate);
        int CountTotalLogs(string directory, DateTime startDate, DateTime endDate);
        IEnumerable<LogEntry> SearchLogsBySize(string directory, long minSize, long maxSize);
        IEnumerable<string> SearchLogsByDirectory(string baseDirectory);
        void UploadLogsToRemoteServer(string apiUrl, string filePath);
    }
}