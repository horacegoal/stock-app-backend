using Microsoft.AspNetCore.Mvc;
using stockAppApi.BLL;
using stockAppApi.Models.Api.Request;
using stockAppApi.Entities;
using Microsoft.AspNetCore.Cors;

namespace stockAppApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    private readonly ITransactionService _transactionService;
    private readonly IYahooScrapStockDataService _yahooScrapStockDataService;
    public StockController(IStockService stockService, IYahooScrapStockDataService yahooScrapStockDataService, ITransactionService transactionService)
    {
        _stockService = stockService;
        _transactionService = transactionService;
        _yahooScrapStockDataService = yahooScrapStockDataService;

    }

    // write a get method to return all stocks from a local list
    [HttpGet("list")]
    public IActionResult Get()
    {
        var stocks = _stockService.ListStocks();
        return Ok(stocks);
    }

    // insert a new Stock using DataContext class
    [HttpPost]
    public IActionResult Post([FromBody] UpsertStockRequest request)
    {

        var stock = _stockService.UpsertStock(request);
        return Ok(stock);
    }

    [HttpPost("UpsertBySymbol/{symbol}")]
    public async Task<IActionResult> UpsertBySymbol(string symbol)
    {
        List<Stock> stocks = await _stockService.GetStocksBySymbols(new string[] { symbol });
        if (stocks.Count == 0)
        {
            return NotFound();
        }
        var data = stocks[0];
        var stock = _stockService.UpsertStock(new UpsertStockRequest()
        {
            Symbol = data.Symbol,
            Name = data.Name,
            Date = data.Date,
            Price = data.Price,
            MarketCap = data.MarketCap,
            PERatio = data.PERatio,
            Volume = data.Volume,
            PercentageChange = data.PercentageChange,
            ShareFloat = data.ShareFloat,
            AverageDailyVolume10Day = data.AverageDailyVolume10Day
        });
        return Ok(stock);
    }

    // [EnableCors("webPortalCorsPolicy")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _stockService.DeleteStock(id);
        return Ok();
    }

    [HttpPost("updateAll")]
    public async Task<IActionResult> UpdateAllStocks()
    {
        var stocks = await _stockService.UpdateAllStocks();

        return Ok(stocks);
    }

    // get transaction list
    [HttpGet("transaction/list")]
    public IActionResult GetTransactionList()
    {
        var transactions = _transactionService.ListTransactions();
        return Ok(transactions);
    }

    [HttpPost("transaction")]
    public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest request)
    {
        var transaction = await _transactionService.AddTransaction(request);
        return Ok(transaction);
    }

    [HttpPost("transaction/translate")]
    public IActionResult TranslateTransactionMessage([FromBody] TranslateTransactionMessageRequest request)
    {
        try
        {
            var transaction = _transactionService.translateTransactionMessage(request.Message);
            return Ok(transaction);
        }
        catch (Exception ex)
        {


            return BadRequest(ex.Message);
        }

        // var transaction = _transactionService.translateTransactionMessage(request.Message);
        // return Ok(transaction);
    }

    [HttpPost("transaction/translateAndAdd")]
    public IActionResult TranslateAndAdd([FromBody] TranslateTransactionMessageRequest request)
    {
        try
        {
            var transaction = _transactionService.translateTransactionMessage(request.Message);
            var addTransactionRequest = new AddTransactionRequest
            {
                StockId = transaction.StockId,
                Type = transaction.Type,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                TransactionDate = transaction.TransactionDate
            };
            var addedTransaction = _transactionService.AddTransaction(addTransactionRequest);
            return Ok(addedTransaction);
        }
        catch (Exception ex)
        {
            Console.WriteLine("exexexexexexexexexexexexexexexex");
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("transaction/totalProfit")]
    public IActionResult GetTotalProfit()
    {
        var totalProfit = _transactionService.calculateTotalProfitFromSellTransactions();
        return Ok(totalProfit);
    }

    [HttpGet("transaction/totalProfitByMonth")]
    public IActionResult GetTotalProfitByMonth(int month, int year)
    {
        var totalProfit = _transactionService.calculateProfitFromSellTransactionsByMonth(month, year);
        return Ok(totalProfit);
    }

    [HttpGet("transaction/totalProfitByMonthList")]
    public IActionResult GetTotalProfitByMonthList()
    {
        var profitByMonthList = _transactionService.ListProfitByMonth();
        return Ok(profitByMonthList);
    }

    [HttpPost("stockHistory")]
    public IActionResult InsertStockHistory([FromBody] InsertStockHistoryRequest request)
    {
        StockHistory sh = new StockHistory
        {
            StockId = request.StockId,
            Date = request.Date,
            ClosePrice = request.ClosePrice,
            Volumn = request.Volumn
        };
        var stockHistory = _stockService.InsertStockHistory(sh);
        return Ok(stockHistory);
    }



    [HttpPost("stockHistory/list")]
    public async Task<IActionResult> InsertStockHistoryList(string symbol)
    {
        int insertCount = await _stockService.InsertStockHistoryList(symbol);
        return Ok(insertCount);
    }

    [HttpGet("stockHistory/list/{symbol}")]
    public IActionResult GetStockHistoryList(string symbol)
    {
        symbol = symbol.ToUpper();
        var stockHistoryList = _stockService.ListStockHistory(symbol);
        object res = new
        {
            stockHistoryList,
            stock = _stockService.GetStockBySymbol(symbol)
        };
        return Ok(res);
    }

    [HttpGet("stockHistory/list/exportCsv/{symbol}")]
    public FileContentResult ExportStockHistoryList(string symbol)
    {
        symbol = symbol.ToUpper();
        return _stockService.ExportStockHistory(symbol);
    }

    // delete stock history by symbol
    [HttpDelete("stockHistory/list/{symbol}")]
    public IActionResult DeleteStockHistoryList(string symbol)
    {
        symbol = symbol.ToUpper();
        _stockService.DeleteStockHistoryBySymbol(symbol);
        return Ok();
    }



    [HttpGet("scrapYahooStocks")]
    public async Task<IActionResult> GetStockDataFromUrl(string screenerId)
    {
        var data = await _yahooScrapStockDataService.GetSymbolFromHtml(screenerId, 100, 0, new List<Stock>());

        // var res = new
        // {
        //     stocks = data.stocks.Select(x => new
        //     {
        //         x.Date,
        //         x.Symbol,
        //         x.Name,
        //         x.Price,
        //         x.MarketCap,
        //         x.PERatio,
        //         x.Volume
        //     })
        //     .ToList(),
        //     count = data.count
        // };

        var res = data.stocks.Select(x => new
        {
            x.Date,
            x.Symbol,
            x.Name,
            x.Price,
            x.MarketCap,
            x.PERatio,
            x.Volume,
            x.PercentageChange,
            x.ShareFloat
        })
            .ToList();

        return Ok(res);
    }

    [HttpGet("scrapYahooStocks/heatmap")]
    public async Task<FileContentResult> GetStockHeatMap(string screenerId)
    {
        var imageData = await _yahooScrapStockDataService.GetStockMarketCapBarChart(screenerId);
        return File(imageData, "image/png");
    }
}
