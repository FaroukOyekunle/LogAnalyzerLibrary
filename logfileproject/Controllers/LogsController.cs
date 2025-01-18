using logfileproject.Interfaces;
using logfileproject.Models;
using Microsoft.AspNetCore.Mvc;

namespace logfileproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

    public LogsController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet("unique-errors")]
    public IActionResult GetUniqueErrors(string directory)
    {
        return Ok(_logService.CountUniqueErrors(directory));
    }

    [HttpGet("duplicated-errors")]
    public IActionResult GetDuplicatedErrors(string directory)
    {
        return Ok(_logService.CountDuplicatedErrors(directory));
    }

    [HttpPost("archive")]
    public IActionResult ArchiveLogs(string directory, DateTime startDate, DateTime endDate)
    {
        _logService.ArchiveLogs(directory, startDate, endDate);
        return Ok("Logs archived successfully.");
    }

    [HttpPost("delete")]
    public IActionResult DeleteLogs(string directory, DateTime startDate, DateTime endDate)
    {
        _logService.DeleteLogs(directory, startDate, endDate);
        return Ok("Logs deleted successfully.");
    }

    [HttpGet("total-logs")]
    public IActionResult CountTotalLogs(string directory, DateTime startDate, DateTime endDate)
    {
        return Ok(_logService.CountTotalLogs(directory, startDate, endDate));
    }

    [HttpGet("search-by-size")]
    public IActionResult SearchLogsBySize(string directory, long minSize, long maxSize)
    {
        return Ok(_logService.SearchLogsBySize(directory, minSize, maxSize));
    }

    [HttpGet("directories")]
    public IActionResult GetDirectories(string baseDirectory)
    {
        return Ok(_logService.SearchLogsByDirectory(baseDirectory));
    }

    [HttpPost("upload")]
    public IActionResult UploadLogs([FromBody] UploadRequest request)
    {
        _logService.UploadLogsToRemoteServer(request.ApiUrl, request.FilePath);
        return Ok("Logs uploaded successfully.");
    }
    }
}