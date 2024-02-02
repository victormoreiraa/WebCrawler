using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Interfaces
{
    public interface IWebCrawlerService
    {
        Task RunCrawlerAsync();
    }
}
