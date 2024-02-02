using System;
using WebCrawler.Interfaces;
using WebCrawler.Models;

namespace WebCrawler.Repositories
{
    public class CrawledDataRepository : ICrawledDataRepository
    {
        private readonly CrawledDbContext _context;

        public CrawledDataRepository()
        {
            _context = new CrawledDbContext();
        }

        public void Save(CrawledData data)
        {
            _context.CrawledData.Add(data);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
