using WebCrawler.Interfaces;
using WebCrawler.Models;
using WebCrawler.Repositories;

namespace WebCrawler.Services
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly ICrawledDataRepository _repository;
        private readonly IRunService _runService;

        public WebCrawlerService(ICrawledDataRepository repository, IRunService runService)
        {
            _repository = repository;
            _runService = runService;
        }

        public async Task RunCrawlerAsync()
        {
            var crawledData = await _runService.CrawlAsync();

            SaveToDatabase(crawledData);
        }

        private void SaveToDatabase(CrawledData data)
        {
            _repository.Save(data);
        }
    }
}
