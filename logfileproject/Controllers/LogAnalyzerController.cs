using LogAnalyzerLibrary;
using Microsoft.AspNetCore.Mvc;

namespace LogAnalyzerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly LogAnalyzer _logAnalyzer;

        public LogsController()
        {
            var directories = new List<string> { @"C:\AmadeoLogs", @"C:\AWIErrors", @"C:\Loggings" };
            _logAnalyzer = new LogAnalyzer(directories);
        }

        [HttpGet("search")]
        public IActionResult SearchLogs([FromQuery] string searchTerm)
        {
            var results = _logAnalyzer.SearchLogs(searchTerm);
            return Ok(results);
        }

        [HttpGet("unique-errors")]
        public IActionResult CountUniqueErrors([FromQuery] string filePath)
        {
            var count = _logAnalyzer.CountUniqueErrors(filePath);
            return Ok(count);
        }

        [HttpGet("duplicated-errors")]
        public IActionResult CountDuplicatedErrors([FromQuery] string filePath)
        {
            var count = _logAnalyzer.CountDuplicatedErrors(filePath);
            return Ok(count);
        }

        [HttpDelete("delete-period")]
        public IActionResult DeleteLogsFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            _logAnalyzer.DeleteLogsFromPeriod(startDate, endDate);
            return NoContent();
        }

        [HttpPost("archive-period")]
        public IActionResult ArchiveLogsFromPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string outputDir)
        {
            _logAnalyzer.ArchiveLogsFromPeriod(startDate, endDate, outputDir);
            return Ok("Logs archived successfully.");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadLogsToServer([FromQuery] string apiUrl, [FromQuery] string filePath)
        {
            await _logAnalyzer.UploadLogsToServer(apiUrl, filePath);
            return Ok("File uploaded successfully.");
        }

        [HttpGet("count-period")]
        public IActionResult CountLogsInPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var count = _logAnalyzer.CountLogsInPeriod(startDate, endDate);
            return Ok(count);
        }

        [HttpGet("search-size")]
        public IActionResult SearchLogsBySize([FromQuery] long minSizeKb, [FromQuery] long maxSizeKb)
        {
            var results = _logAnalyzer.SearchLogsBySize(minSizeKb, maxSizeKb);
            return Ok(results);
        }
    }
}
