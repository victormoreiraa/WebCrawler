using System;

namespace WebCrawler.Models
{
    public class CrawledData
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PagesCount { get; set; }
        public int RowsCount { get; set; }
        public string? JsonFilePath { get; set; }
    }
}
