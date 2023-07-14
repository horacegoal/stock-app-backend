using System;
namespace stockAppApi.BLL
{
    public interface IScrapStockDataService
    {
        Task<string> GetHtml(string url);

    }
    public class ScrapStockDataService : IScrapStockDataService
    {
        public ScrapStockDataService()
        {

        }

        public Task<string> GetHtml(string url)
        {
            var client = new HttpClient();
            return client.GetStringAsync(url);
        }
    }
}

