using Microsoft.EntityFrameworkCore;
using WebCrawler.Utils;

namespace WebCrawler.Models
{
    public class CrawledDbContext : DbContext
    {
        public DbSet<CrawledData> CrawledData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Constants.ConnectionString);
        }
    }
}
