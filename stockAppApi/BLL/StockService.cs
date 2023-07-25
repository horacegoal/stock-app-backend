using System;
using System.Text.Json;
using FluentValidation;
using stockAppApi.BLL.Validators;
using stockAppApi.Entities;
using stockAppApi.Models.Api.Request;
using stockAppApi.Models.Api.Response;
using Microsoft.Extensions.Configuration;

namespace stockAppApi.BLL
{
    // create interface for the stock service
    public interface IStockService
    {
        Stock UpsertStock(UpsertStockRequest request);
        List<Stock> ListStocks();
        Task<Stock?> GetStockBySymbol(string symbol);
    }
    public class StockService : IStockService
    {
        private readonly DataContext _dbContext;
        private readonly string _apiKey;

        protected readonly IConfiguration _configuration;

        public StockService(DataContext context, IConfiguration configuration)
        {
            _dbContext = context;

            if (configuration != null && configuration["ApiKey"] != null)
            {
                _apiKey = configuration.GetValue<string>("ApiKey") ?? "";
                Console.WriteLine(_apiKey);
            }
            else
            {
                throw new Exception("ApiKey not found in appsettings.json");
            }
        }

        public Stock UpsertStock(UpsertStockRequest request)
        {
            //validate the request
            var validator = new UpsertStockValidator();
            validator.ValidateAndThrow(request);

            // check if the stock exists
            var stock = _dbContext.Stocks.FirstOrDefault(x => x.Symbol == request.Symbol);
            if (stock == null)
            {
                stock = new Stock
                {
                    Symbol = request.Symbol,
                    Name = request.Name,
                    Date = request.Date,
                    Price = request.Price,
                    MarketCap = request.MarketCap,
                    PERatio = request.PERatio,
                    Volume = request.Volume,
                    PercentageChange = request.PercentageChange,
                    ShareFloat = request.ShareFloat
                };
                _dbContext.Stocks.Add(stock);
            }
            else
            {
                stock.Symbol = request.Symbol;
                stock.Name = request.Name;
                stock.Date = request.Date;
                stock.Price = request.Price;
                stock.MarketCap = request.MarketCap;
                stock.PERatio = request.PERatio;
                stock.Volume = request.Volume;
                stock.PercentageChange = request.PercentageChange;
                stock.ShareFloat = request.ShareFloat;
            }

            _dbContext.SaveChanges();
            return stock;
        }

        // list stocks from the database
        public List<Stock> ListStocks()
        {
            var stocks = _dbContext.Stocks.ToList();
            return stocks;
        }

        public async Task<Stock?> GetStockBySymbol(string symbol)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/AAPL"),
                Headers =
                    {
                        { "X-RapidAPI-Key", _apiKey },
                        { "X-RapidAPI-Host", "yahoo-finance15.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                List<GetStockDataBySymbolResponse>? stockDataList = JsonSerializer.Deserialize<List<GetStockDataBySymbolResponse>>(body);
                if (stockDataList?[0] != null)
                {
                    var stock = new Stock()
                    {
                        Symbol = stockDataList[0].symbol,
                        Name = stockDataList[0].longName,
                        // Date = stockDataList[0].Date,
                        Price = stockDataList[0].regularMarketPrice,
                        MarketCap = stockDataList[0].marketCap,
                        PERatio = stockDataList[0].epsTrailingTwelveMonths != 0 ? stockDataList[0].regularMarketPrice / stockDataList[0].epsTrailingTwelveMonths : 0,
                        Volume = stockDataList[0].regularMarketVolume,
                        PercentageChange = stockDataList[0].regularMarketChangePercent,
                        ShareFloat = stockDataList[0].sharesOutstanding
                    };
                    return stock;

                }
                else
                {
                    return null;
                }


            }
        }
    }
}

