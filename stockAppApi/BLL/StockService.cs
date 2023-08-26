using System;
using System.Text.Json;
using FluentValidation;
using stockAppApi.BLL.Validators;
using stockAppApi.Entities;
using stockAppApi.Models.Api.Request;
using stockAppApi.Models.Api.Response;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace stockAppApi.BLL
{
    // create interface for the stock service
    public interface IStockService
    {
        Stock GetStockBySymbol(string symbol);
        Stock UpsertStock(UpsertStockRequest request);
        List<Stock> ListStocks();
        Task<List<Stock>> GetStocksBySymbols(string[] symbols);

        void DeleteStock(int id);
        Task<List<Stock>> UpdateAllStocks();
        StockHistory InsertStockHistory(StockHistory stockHistory);
        Task<int> InsertStockHistoryList(string symbol);
        List<StockHistory> ListStockHistory(string symbol);
        FileContentResult ExportStockHistory(string symbol);

        void DeleteStockHistoryBySymbol(string symbol);

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
                    Date = DateTimeOffset.UtcNow.DateTime,
                    Price = request.Price,
                    MarketCap = request.MarketCap,
                    PERatio = request.PERatio,
                    Volume = request.Volume,
                    PercentageChange = request.PercentageChange,
                    ShareFloat = request.ShareFloat,
                    AverageDailyVolume10Day = request.AverageDailyVolume10Day
                };
                _dbContext.Stocks.Add(stock);
            }
            else
            {
                stock.Symbol = request.Symbol;
                stock.Name = request.Name;
                stock.Date = DateTimeOffset.UtcNow.DateTime;
                stock.Price = request.Price;
                stock.MarketCap = request.MarketCap;
                stock.PERatio = request.PERatio;
                stock.Volume = request.Volume;
                stock.PercentageChange = request.PercentageChange;
                stock.ShareFloat = request.ShareFloat;
                stock.AverageDailyVolume10Day = request.AverageDailyVolume10Day;
            }

            _dbContext.SaveChanges();
            return stock;
        }

        // get stock by symbol
        public Stock GetStockBySymbol(string symbol)
        {
            var stock = _dbContext.Stocks.FirstOrDefault(x => x.Symbol == symbol);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }
            return stock;
        }


        // list stocks from the database
        public List<Stock> ListStocks()
        {
            var stocks = _dbContext.Stocks.Select
            (
                x => new Stock
                {
                    Id = x.Id,
                    Symbol = x.Symbol,
                    Name = x.Name,
                    Date = x.Date,
                    Price = x.Price,
                    MarketCap = x.MarketCap,
                    PERatio = x.PERatio,
                    Volume = x.Volume,
                    PercentageChange = x.PercentageChange,
                    ShareFloat = x.ShareFloat,
                    AverageDailyVolume10Day = x.AverageDailyVolume10Day,
                    ShareCount = getShareCountByStock(x.Id, _dbContext),
                    CostPerShare = getCostPerShare(x.Id, _dbContext)
                }
            ).ToList();
            return stocks;

        }

        public static int getShareCountByStock(int stockId, DataContext _dbContext)
        {
            var transaction = _dbContext.Transactions.Where(x => x.StockId == stockId).ToList();
            int shareCount = 0;
            foreach (var item in transaction)
            {
                if (item.Type == TransactionType.Buy)
                {
                    shareCount += item.Quantity;
                }
                else
                {
                    shareCount -= item.Quantity;
                }
            }
            return shareCount;
        }

        public static double getCostPerShare(int stockId, DataContext _dbContext)
        {
            var transactions = _dbContext.Transactions.Where(x => x.StockId == stockId).ToList();
            double costPerShare = 0;
            int totalShares = 0;
            transactions.ForEach(x =>
            {
                if (x.Type == TransactionType.Buy)
                {
                    costPerShare = (costPerShare * totalShares + x.Price * x.Quantity) / (totalShares + x.Quantity);
                    totalShares += x.Quantity;
                }

                else if (x.Type == TransactionType.Sell)
                {
                    totalShares -= x.Quantity;
                }
            });
            return costPerShare;
        }

        public async Task<List<Stock>> UpdateAllStocks()
        {
            var stocks = _dbContext.Stocks.ToList();

            string[] symbols = stocks.Select(x => x.Symbol).ToArray()!;

            List<Stock> updatedStocks = await GetStocksBySymbols(symbols);
            foreach (var stock in updatedStocks)
            {
                var stockToUpdate = _dbContext.Stocks.FirstOrDefault(x => x.Symbol == stock.Symbol);
                if (stockToUpdate != null)
                {
                    stockToUpdate.Name = stock.Name;
                    stockToUpdate.Date = DateTimeOffset.UtcNow.DateTime;
                    stockToUpdate.Price = stock.Price;
                    stockToUpdate.MarketCap = stock.MarketCap;
                    stockToUpdate.PERatio = stock.PERatio;
                    stockToUpdate.Volume = stock.Volume;
                    stockToUpdate.PercentageChange = stock.PercentageChange;
                    stockToUpdate.ShareFloat = stock.ShareFloat;
                    stockToUpdate.AverageDailyVolume10Day = stock.AverageDailyVolume10Day;

                }
            }


            _dbContext.SaveChanges();
            return ListStocks();
        }

        // Delete Stock
        public void DeleteStock(int id)
        {
            var stock = _dbContext.Stocks.FirstOrDefault(x => x.Id == id);
            if (stock != null)
            {
                _dbContext.Stocks.Remove(stock);
                _dbContext.SaveChanges();
            }
        }

        public List<StockHistory> ListStockHistory(string symbol)
        {
            var stockHistories = _dbContext.StockHistories
                .Where(x => x.Stock.Symbol == symbol)
                .ToList();
            return stockHistories;
        }

        public StockHistory InsertStockHistory(StockHistory stockHistory)
        {
            _dbContext.StockHistories.Add(stockHistory);
            _dbContext.SaveChanges();
            return stockHistory;
        }

        public void DeleteStockHistoryBySymbol(string symbol)
        {
            var stock = _dbContext.Stocks.FirstOrDefault(x => x.Symbol == symbol);
            if (stock != null)
            {
                var stockHistories = _dbContext.StockHistories.Where(x => x.StockId == stock.Id).ToList();
                _dbContext.StockHistories.RemoveRange(stockHistories);
                _dbContext.SaveChanges();
            }
        }

        public async Task<int> InsertStockHistoryList(string symbol)
        {
            string interval = "1d";
            var stock = _dbContext.Stocks.FirstOrDefault(x => x.Symbol == symbol);
            if (stock == null)
            {
                throw new Exception("Stock not found");
            }
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yahoo-finance15.p.rapidapi.com/api/yahoo/hi/history/{symbol}/{interval}?diffandsplits=false"),
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
                // Console.WriteLine(body);
                GetStockHistoryResponse? stockHistoryData = JsonSerializer.Deserialize<GetStockHistoryResponse>(body, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                if (stockHistoryData == null)
                {
                    throw new Exception("Stock history data not found");
                }

                List<StockHistory> stockHistories = new List<StockHistory>();

                int count = 0;
                Console.WriteLine(stockHistoryData);
                foreach (long timestamp in stockHistoryData.items.Keys.Reverse())
                {
                    var data = new StockHistory()
                    {
                        StockId = stock.Id,
                        Date = DateTime.Parse(stockHistoryData.items[timestamp].date),
                        ClosePrice = stockHistoryData.items[timestamp].close,
                        Volumn = stockHistoryData.items[timestamp].volume
                    };
                    //check if the stock history exists
                    var stockHistory = _dbContext.StockHistories.FirstOrDefault(x => x.StockId == stock.Id && x.Date == data.Date);
                    if (stockHistory == null)
                    {
                        stockHistories.Add(data);
                        count++;

                    }
                    if (count > 365)
                    {
                        break;

                    }
                }
                _dbContext.StockHistories.AddRange(stockHistories);
                _dbContext.SaveChanges();
                return count;
            }
        }

        // export stock history as csv by symbol
        public FileContentResult ExportStockHistory(string symbol)
        {
            var stockHistories = _dbContext.StockHistories
                .Where(x => x.Stock.Symbol == symbol)
                .ToList();
            var csv = new StringBuilder();
            csv.AppendLine("Date,ClosePrice,Volumn");
            foreach (var stockHistory in stockHistories)
            {
                var newLine = string.Format("{0},{1},{2}", stockHistory.Date, stockHistory.ClosePrice, stockHistory.Volumn);
                csv.AppendLine(newLine);
            }
            return new FileContentResult(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv") { FileDownloadName = $"{symbol}.csv" };
        }


        public async Task<List<Stock>> GetStocksBySymbols(string[] symbols)
        {
            string symbolsString = string.Join(",", symbols);
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{symbolsString}"),
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
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement list = doc.RootElement;
                List<Stock> stocks = new List<Stock>();
                // loop through the list
                foreach (JsonElement stockObj in list.EnumerateArray())
                {
                    var stock = new Stock()
                    {
                        Symbol = stockObj.GetProperty("symbol").GetString()!,
                        Name = stockObj.GetProperty("longName").GetString()!,
                        // stock.Date = DateTimeOffset.UtcNow.DateTime;
                        Price = stockObj.GetProperty("regularMarketPrice").GetDouble(),
                        MarketCap = stockObj.GetProperty("marketCap").GetInt64(),
                        PERatio = stockObj.GetProperty("epsTrailingTwelveMonths").GetDouble() != 0 ? stockObj.GetProperty("regularMarketPrice").GetDouble() / stockObj.GetProperty("epsTrailingTwelveMonths").GetDouble() : 0,
                        Volume = stockObj.GetProperty("regularMarketVolume").GetInt32(),
                        PercentageChange = stockObj.GetProperty("regularMarketChangePercent").GetDouble(),
                        ShareFloat = stockObj.GetProperty("sharesOutstanding").GetInt64(),
                        AverageDailyVolume10Day = stockObj.GetProperty("averageDailyVolume10Day").GetInt32(),
                    };
                    stocks.Add(stock);

                }
                // List<GetStockDataBySymbolResponse>? stockDataList = JsonSerializer.Deserialize<List<GetStockDataBySymbolResponse>>(body, new JsonSerializerOptions
                // {
                //     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                // });
                // foreach (var stockData in stockDataList)
                // {
                //     var stock = new Stock()
                //     {
                //         Symbol = stockData.symbol,
                //         Name = stockData.longName,
                //         // Date = stockData.Date,
                //         Price = stockData.regularMarketPrice,
                //         MarketCap = stockData.marketCap,
                //         PERatio = stockData.epsTrailingTwelveMonths != 0 ? stockData.regularMarketPrice / stockData.epsTrailingTwelveMonths : 0,
                //         Volume = stockData.regularMarketVolume,
                //         PercentageChange = stockData.regularMarketChangePercent,
                //         ShareFloat = stockData.sharesOutstanding,
                //         AverageDailyVolume10Day = stockData.averageDailyVolume10Day
                //     };
                //     stocks.Add(stock);
                // }
                return stocks;



            }
        }
    }
}

