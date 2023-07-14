using Microsoft.AspNetCore.Mvc;
using stockAppApi.BLL;
using stockAppApi.Models.Api.Request;

namespace stockAppApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly IScrapStockDataService _scrapStockDataService;
    public StockController(IStockService stockService, IScrapStockDataService scrapStockDataService)
    {
        _stockService = stockService;
        _scrapStockDataService = scrapStockDataService;

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
    public IActionResult Post([FromBody] InsertStockRequest request)
    {

        var stock = _stockService.InsertStock(request);
        return Ok(stock);
    }
    [HttpPost("scrap")]
    public async Task<IActionResult> GetStockDataFromUrl([FromBody] string requestUrl)
    {
        var data = await _scrapStockDataService.GetHtml(requestUrl);
        return Ok(data);
    }
}
