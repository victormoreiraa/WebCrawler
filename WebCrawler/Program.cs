using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Interfaces;
using WebCrawler.Models;
using WebCrawler.Repositories;
using WebCrawler.Services;

class Program
{
    static async Task Main()
    {
        // Injeção de dependência
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICrawledDataRepository, CrawledDataRepository>()
            .AddScoped<IRunService, RunService>()
            .AddScoped<IWebCrawlerService, WebCrawlerService>()  
            .AddScoped<CrawledDbContext>()
            .BuildServiceProvider();

        
        var webCrawlerService = serviceProvider.GetService<IWebCrawlerService>();

       
        if (webCrawlerService != null)
        {
            await webCrawlerService.RunCrawlerAsync();
        }
        else
        {
            Console.WriteLine("Falha ao resolver o serviço.");
        }
    }

}
