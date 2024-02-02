using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebCrawler.Interfaces;
using WebCrawler.Models;

public class RunService : IRunService
{
    private readonly IWebDriver _driver;
    private int _pageCounter = 1;
    private int _totalPages = 0;
    private int _totalRows = 0;
    private List<Dictionary<string, string>> _allData = new List<Dictionary<string, string>>();
    private string _jsonFileName = string.Empty;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(3);
    private DateTime _startTime;

    public RunService()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);
        chromeOptions.AddUserProfilePreference("profile.managed_default_content_settings.javascript", 2);
        _driver = new ChromeDriver(chromeOptions);
    }

    public async Task<CrawledData> CrawlAsync()
    {
        try
        {
            _startTime = DateTime.Now;
            await _semaphore.WaitAsync();
            await NavigateToSiteAsync();

            while (true)
            {
                var extractedData = await ExtractDataFromPageAsync();
                await TakeHtmlScreenshotAsync();

                _totalPages++;
                _allData.AddRange(extractedData);

                Thread.Sleep(5000);

                if (!GoToNextPage())
                {
                    break;
                }
            }

            await SaveAllDataToJsonAsync();

            return new CrawledData
            {
                StartTime = _startTime,
                EndTime = DateTime.Now,
                PagesCount = _totalPages,
                RowsCount = _totalRows,
                JsonFilePath = _jsonFileName
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção: {ex.Message}");
            throw;
        }
        finally
        {
            _semaphore.Release();
            _driver.Quit();
        }
    }

    private async Task SaveAllDataToJsonAsync()
    {
        string folderPath = @"C:\WebCrawler\WebCrawler\DataExtractions";
        _jsonFileName = $"dataExtraction_{DateTime.Now:yyyyMMdd_HHmmssfff}.json";
        string jsonFilePath = Path.Combine(folderPath, _jsonFileName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        await _semaphore.WaitAsync();

        try
        {
            File.WriteAllText(jsonFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(_allData));
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private bool GoToNextPage()
    {
        try
        {
            var nextPageLink = _driver.FindElement(By.XPath("//li[@class='page-item active']/following-sibling::li[@class='page-item']/a"));
            nextPageLink.Click();

            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    private async Task NavigateToSiteAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            await Task.Run(() => _driver.Navigate().GoToUrl("https://proxyservers.pro/proxy/list/order/updated/order_dir/asc"));
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<Dictionary<string, string>>> ExtractDataFromPageAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            List<Dictionary<string, string>> extractedData = new List<Dictionary<string, string>>();
            var rows = await Task.Run(() => _driver.FindElements(By.XPath("//table[@class='table table-hover']/tbody/tr")));

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.XPath("td"));

                if (cells.Count == 8)
                {
                    string ipAddress = cells[1].FindElement(By.TagName("a")).Text;
                    string port = cells[2].FindElement(By.ClassName("port")).GetAttribute("data-port");
                    string country = cells[3].Text;
                    string protocol = cells[6].Text;

                    var rowData = new Dictionary<string, string>
                    {
                        {"IP Address", ipAddress},
                        {"Port", port},
                        {"Country", country},
                        {"Protocol", protocol}
                    };

                    extractedData.Add(rowData);
                    _totalRows++;
                }
            }

            return extractedData;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task TakeHtmlScreenshotAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            string htmlContent = await Task.Run(() => _driver.PageSource);

            string folderPath = @"C:\WebCrawler\WebCrawler\Pages";
            string htmlFileName = $"page{_pageCounter}_{DateTime.Now:yyyyMMdd_HHmmssfff}.html";
            string htmlFilePath = Path.Combine(folderPath, htmlFileName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(htmlFilePath, htmlContent);
            _pageCounter++;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
