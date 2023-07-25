// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
namespace stockAppApi.Models.Api.Response;
public class DividendDate
{
    public int timestamp { get; set; }
    public string date { get; set; }
    public int timezone_type { get; set; }
    public string timezone { get; set; }
}

public class EarningsTimestamp
{
    public int timestamp { get; set; }
    public string date { get; set; }
    public int timezone_type { get; set; }
    public string timezone { get; set; }
}

public class EarningsTimestampEnd
{
    public int timestamp { get; set; }
    public string date { get; set; }
    public int timezone_type { get; set; }
    public string timezone { get; set; }
}

public class EarningsTimestampStart
{
    public int timestamp { get; set; }
    public string date { get; set; }
    public int timezone_type { get; set; }
    public string timezone { get; set; }
}

public class RegularMarketTime
{
    public int timestamp { get; set; }
    public string date { get; set; }
    public int timezone_type { get; set; }
    public string timezone { get; set; }
}

public class GetStockDataBySymbolResponse
{
    public double ask { get; set; }
    public int askSize { get; set; }
    public int averageDailyVolume10Day { get; set; }
    public int averageDailyVolume3Month { get; set; }
    public double bid { get; set; }
    public int bidSize { get; set; }
    public double bookValue { get; set; }
    public string currency { get; set; }
    public DividendDate dividendDate { get; set; }
    public EarningsTimestamp earningsTimestamp { get; set; }
    public EarningsTimestampStart earningsTimestampStart { get; set; }
    public EarningsTimestampEnd earningsTimestampEnd { get; set; }
    public double epsForward { get; set; }
    public double epsTrailingTwelveMonths { get; set; }
    public string exchange { get; set; }
    public int exchangeDataDelayedBy { get; set; }
    public string exchangeTimezoneName { get; set; }
    public string exchangeTimezoneShortName { get; set; }
    public double fiftyDayAverage { get; set; }
    public double fiftyDayAverageChange { get; set; }
    public double fiftyDayAverageChangePercent { get; set; }
    public double fiftyTwoWeekHigh { get; set; }
    public double fiftyTwoWeekHighChange { get; set; }
    public double fiftyTwoWeekHighChangePercent { get; set; }
    public double fiftyTwoWeekLow { get; set; }
    public double fiftyTwoWeekLowChange { get; set; }
    public double fiftyTwoWeekLowChangePercent { get; set; }
    public string financialCurrency { get; set; }
    public double forwardPE { get; set; }
    public string fullExchangeName { get; set; }
    public int gmtOffSetMilliseconds { get; set; }
    public string language { get; set; }
    public string longName { get; set; }
    public string market { get; set; }
    public long marketCap { get; set; }
    public string marketState { get; set; }
    public string messageBoardId { get; set; }
    public object postMarketChange { get; set; }
    public object postMarketChangePercent { get; set; }
    public object postMarketPrice { get; set; }
    public object postMarketTime { get; set; }
    public int priceHint { get; set; }
    public double priceToBook { get; set; }
    public string quoteSourceName { get; set; }
    public string quoteType { get; set; }
    public double regularMarketChange { get; set; }
    public double regularMarketChangePercent { get; set; }
    public double regularMarketDayHigh { get; set; }
    public double regularMarketDayLow { get; set; }
    public double regularMarketOpen { get; set; }
    public double regularMarketPreviousClose { get; set; }
    public double regularMarketPrice { get; set; }
    public RegularMarketTime regularMarketTime { get; set; }
    public int regularMarketVolume { get; set; }
    public long sharesOutstanding { get; set; }
    public string shortName { get; set; }
    public int sourceInterval { get; set; }
    public string symbol { get; set; }
    public bool tradeable { get; set; }
    public double trailingAnnualDividendRate { get; set; }
    public double trailingAnnualDividendYield { get; set; }
    public double trailingPE { get; set; }
    public double twoHundredDayAverage { get; set; }
    public double twoHundredDayAverageChange { get; set; }
    public double twoHundredDayAverageChangePercent { get; set; }
}

