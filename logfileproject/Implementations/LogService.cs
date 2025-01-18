using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using logfileproject.Interfaces;
using logfileproject.Models;

namespace logfileproject.Implementations
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public int CountUniqueErrors(string directory)
    {
        var logs = _logRepository.GetLogs(directory, null, null);
        return logs.Where(log => log.Level == "Error").Select(log => log.Message).Distinct().Count();
    }

    public int CountDuplicatedErrors(string directory)
    {
        var logs = _logRepository.GetLogs(directory, null, null);
        return logs.Where(log => log.Level == "Error")
                   .GroupBy(log => log.Message)
                   .Where(group => group.Count() > 1)
                   .Count();
    }

    public void ArchiveLogs(string directory, DateTime startDate, DateTime endDate)
    {
        _logRepository.ArchiveLogs(directory, startDate, endDate);
    }

    public void DeleteLogs(string directory, DateTime startDate, DateTime endDate)
    {
        _logRepository.DeleteLogs(directory, startDate, endDate);
    }

    public int CountTotalLogs(string directory, DateTime startDate, DateTime endDate)
    {
        return _logRepository.GetLogs(directory, startDate, endDate).Count();
    }

    public IEnumerable<LogEntry> SearchLogsBySize(string directory, long minSize, long maxSize)
    {
        return _logRepository.SearchLogsBySize(directory, minSize, maxSize);
    }

    public IEnumerable<string> SearchLogsByDirectory(string baseDirectory)
    {
        return _logRepository.GetDirectories(baseDirectory);
    }

    public void UploadLogsToRemoteServer(string apiUrl, string filePath)
    {
        using var client = new HttpClient();
        var content = new MultipartFormDataContent
        {
            { new ByteArrayContent(File.ReadAllBytes(filePath)), "file", Path.GetFileName(filePath) }
        };

        var result = client.PostAsync(apiUrl, content).Result;
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to upload logs. Status Code: {result.StatusCode}");
        }
    }
    }
}