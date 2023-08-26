// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
namespace stockAppApi.Models.Api.Response;
public class GetStockHistoryResponse
{
    public Meta meta { get; set; }
    public Dictionary<long, StockDataByTimestamp> items { get; set; }
    public dynamic? error { get; set; }
}

public class Meta
{
    public string currency { get; set; }
    public string symbol { get; set; }
    public string exchangeName { get; set; }
    public string instrumentType { get; set; }
    public int firstTradeDate { get; set; }
    public int regularMarketTime { get; set; }
    public int gmtoffset { get; set; }
    public string timezone { get; set; }
    public string exchangeTimezoneName { get; set; }
    public double regularMarketPrice { get; set; }
    public double chartPreviousClose { get; set; }
    public int priceHint { get; set; }
    public string dataGranularity { get; set; }
    public string range { get; set; }
}

public class StockDataByTimestamp
{
    public string date { get; set; }
    public int date_utc { get; set; }
    public double open { get; set; }
    public double high { get; set; }
    public double low { get; set; }
    public double close { get; set; }
    public long volume { get; set; }
    public double adjclose { get; set; }
}