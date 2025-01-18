using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace logfileproject.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; } // Error, Warning, Info
    }
}