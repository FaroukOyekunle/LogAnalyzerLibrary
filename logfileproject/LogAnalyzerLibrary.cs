using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LogAnalyzerLibrary
{
    public class LogAnalyzer
    {
        private readonly IEnumerable<string> _directories;

        public LogAnalyzer(IEnumerable<string> directories)
        {
            _directories = directories ?? throw new ArgumentNullException(nameof(directories));
        }

        public IEnumerable<string> SearchLogs(string searchTerm)
        {
            foreach (var dir in _directories)
            {
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        foreach (var line in File.ReadLines(file).Where(line => line.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        {
                            yield return line;
                        }
                    }
                }
            }
        }

        public int CountUniqueErrors(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllLines(filePath).Distinct().Count() : 0;
        }

        public int CountDuplicatedErrors(string filePath)
        {
            return File.Exists(filePath) 
                ? File.ReadAllLines(filePath).GroupBy(line => line).Count(group => group.Count() > 1) 
                : 0;
        }

        public void DeleteLogsFromPeriod(DateTime startDate, DateTime endDate)
        {
            foreach (var dir in _directories)
            {
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (File.GetLastWriteTime(file).Date >= startDate.Date && File.GetLastWriteTime(file).Date <= endDate.Date)
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
        }

        public void ArchiveLogsFromPeriod(DateTime startDate, DateTime endDate, string outputDir)
        {
            var tempDir = Path.Combine(outputDir, "TempLogs");
            Directory.CreateDirectory(tempDir);
            bool hasValidFile = false;
            foreach (var dir in _directories)
            {
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (File.GetLastWriteTime(file) >= startDate && File.GetLastWriteTime(file) <= endDate)
                        {
                            hasValidFile = true;
                            File.Copy(file, Path.Combine(tempDir, Path.GetFileName(file)), true);
                            File.Delete(file);
                        }
                    }
                }
            }
            var zipFileName = Path.Combine(outputDir, $"{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.zip");
            if(hasValidFile && !File.Exists(zipFileName) )
            {
                ZipFile.CreateFromDirectory(tempDir, zipFileName);
                Directory.Delete(tempDir, true);
            }
       
        }

        public async Task UploadLogsToServer(string apiUrl, string filePath)
        {
            using var httpClient = new HttpClient();
            using var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(await File.ReadAllBytesAsync(filePath)), "file", Path.GetFileName(filePath) }
            };

            var response = await httpClient.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public int CountLogsInPeriod(DateTime startDate, DateTime endDate)
        {
            return _directories.Where(Directory.Exists).SelectMany(dir => 
                Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories)
                .Where(file => File.GetLastWriteTime(file) >= startDate && File.GetLastWriteTime(file) <= endDate)).Count();
        }

        public IEnumerable<string> SearchLogsBySize(long minSizeKb, long maxSizeKb)
        {
            return _directories.Where(Directory.Exists).SelectMany(dir => 
                Directory.GetFiles(dir, "*.log", SearchOption.AllDirectories)
                .Where(file => new FileInfo(file).Length / 1024 >= minSizeKb && new FileInfo(file).Length / 1024 <= maxSizeKb));
        }
    }
}
