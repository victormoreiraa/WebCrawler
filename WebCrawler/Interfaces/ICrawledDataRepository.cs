using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Interfaces
{
    public interface ICrawledDataRepository : IDisposable
    {
        void Save(CrawledData data);
    }
}
