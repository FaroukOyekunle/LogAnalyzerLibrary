using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using logfileproject.Interfaces;
using logfileproject.Models;

namespace logfileproject.Implementations
{
    public class LogRepository : ILogRepository
    {
        public IEnumerable<LogEntry> GetLogs(string directory, DateTime? startDate, DateTime? endDate)
        {
            var logFiles = Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories);
            var logEntries = new List<LogEntry>();

            foreach (var file in logFiles)
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (DateTime.TryParse(line.Substring(0, 19), out var timestamp))
                    {
                        if (!startDate.HasValue || !endDate.HasValue ||
                            (timestamp >= startDate && timestamp <= endDate))
                        {
                            logEntries.Add(new LogEntry
                            {
                                Timestamp = timestamp,
                                Message = line.Substring(20),
                                Level = ParseLogLevel(line)
                            });
                        }
                    }
                }
            }

            return logEntries;
        }

        public void ArchiveLogs(string directory, DateTime startDate, DateTime endDate)
        {
            var logsToArchive = GetLogs(directory, startDate, endDate);
            var archiveName = $"{startDate:dd_MM_yyyy}-{endDate:dd_MM_yyyy}.zip";
            var archivePath = Path.Combine(directory, archiveName);

            using var zip = ZipFile.Open(archivePath, ZipArchiveMode.Create);
            foreach (var file in Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(file).Where(line =>
                {
                    if (DateTime.TryParse(line.Substring(0, 19), out var timestamp))
                    {
                        return timestamp >= startDate && timestamp <= endDate;
                    }
                    return false;
                });

                if (lines.Any())
                {
                    var tempFile = Path.GetTempFileName();
                    File.WriteAllLines(tempFile, lines);
                    zip.CreateEntryFromFile(tempFile, Path.GetFileName(file));
                    File.Delete(tempFile);
                }
            }
        }

        public void DeleteLogs(string directory, DateTime startDate, DateTime endDate)
        {
            foreach (var file in Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(file).Where(line =>
                {
                    if (DateTime.TryParse(line.Substring(0, 19), out var timestamp))
                    {
                        return !(timestamp >= startDate && timestamp <= endDate);
                    }
                    return true;
                }).ToList();

                if (lines.Count == 0)
                {
                    File.Delete(file);
                }
                else
                {
                    File.WriteAllLines(file, lines);
                }
            }
        }

        public IEnumerable<LogEntry> SearchLogsBySize(string directory, long minSize, long maxSize)
        {
            return Directory.GetFiles(directory, "*.log", SearchOption.AllDirectories)
                .Where(file => new FileInfo(file).Length >= minSize * 1024 &&
                               new FileInfo(file).Length <= maxSize * 1024)
                .SelectMany(file =>
                {
                    var lines = File.ReadAllLines(file);
                    return lines.Select(line =>
                    {
                        if (DateTime.TryParse(line.Substring(0, 19), out var timestamp))
                        {
                            return new LogEntry
                            {
                                Timestamp = timestamp,
                                Message = line.Substring(20),
                                Level = ParseLogLevel(line)
                            };
                        }
                        return null;
                    }).Where(log => log != null);
                });
        }

        public IEnumerable<string> GetDirectories(string baseDirectory)
        {
            return Directory.GetDirectories(baseDirectory, "*", SearchOption.AllDirectories);
        }

        private string ParseLogLevel(string logLine)
        {
            if (logLine.Contains("[ERROR]"))
                return "Error";
            if (logLine.Contains("[WARNING]"))
                return "Warning";
            if (logLine.Contains("[INFO]"))
                return "Info";

            return "Unknown";
        }
    }
}