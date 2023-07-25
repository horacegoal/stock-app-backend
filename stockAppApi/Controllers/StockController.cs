using Microsoft.AspNetCore.Mvc;
using stockAppApi.BLL;
using stockAppApi.Models.Api.Request;
using stockAppApi.Entities;

namespace stockAppApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly IYahooScrapStockDataService _yahooScrapStockDataService;
    public StockController(IStockService stockService, IYahooScrapStockDataService yahooScrapStockDataService)
    {
        _stockService = stockService;
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
        Stock data = await _stockService.GetStockBySymbol(symbol);
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
            ShareFloat = data.ShareFloat
        });
        return Ok(stock);
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
