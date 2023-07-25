using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using stockAppApi.Entities;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Core.Drawing;
using System.Net.Http.Headers;
using System.Text.Json;

namespace stockAppApi.BLL
{
    public interface IYahooScrapStockDataService
    {
        Task<string> GetYahooStockHtml(int count, int offset, string id);
        Task<(List<Stock> stocks, int count)> GetSymbolFromHtml(string screenerId, int count, int offset, List<Stock> stocks);
        Task<byte[]> GetStockMarketCapBarChart(string screenerId);
    }

    public class YahooScrapStockDataService : ScrapStockDataService, IYahooScrapStockDataService
    {

        public YahooScrapStockDataService()
        {
        }

        public string GetYahooStockKeyStatisticUrl(string symbol)
        {
            return $"https://finance.yahoo.com/quote/{symbol}/key-statistics";
        }

        public Task<string> GetYahooStockKeyStatisticHtml(string symbol)
        {
            string url = GetYahooStockKeyStatisticUrl(symbol);

            return GetHtml(url);
        }

        public async Task<double> GetStockShareFloat(string symbol)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{symbol}/default-key-statistics"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "98b250e0fdmsh2fef79af7e1ab50p1f6207jsnc032ff321b6b" },
                        { "X-RapidAPI-Host", "yahoo-finance15.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                JsonDocument doc = JsonDocument.Parse(body);
                doc.RootElement.EnumerateObject().FirstOrDefault().Value.GetProperty("sharesOutstanding").GetProperty("raw").TryGetDouble(out double shareFloat);
                return shareFloat;

            }
        }

        public Task<string> GetYahooStockHtml(int count, int offset, string screenerId)
        {
            string url = $"https://finance.yahoo.com/screener/unsaved/{screenerId}?count={count}&offset={offset}";

            return GetHtml(url);
        }

        public async Task<(List<Stock> stocks, int count)> GetSymbolFromHtml(string screenerId, int count, int offset, List<Stock> stocks)
        {
            var html = await GetYahooStockHtml(count, offset, screenerId);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var tbody = htmlDocument.DocumentNode.SelectSingleNode("//tbody");
            if (tbody != null && tbody.InnerHtml != string.Empty)
            {
                var trs = tbody.SelectNodes(".//tr");
                Console.WriteLine(trs.Count);
                if (trs != null)
                {
                    foreach (var tr in trs)
                    {
                        var stock = new Stock();
                        stock.Date = DateTime.Now;
                        var symbolTag = tr.SelectSingleNode(".//td/a");
                        if (symbolTag != null)
                        {
                            stock.Symbol = symbolTag.InnerHtml;

                            double shareFloat = await GetStockShareFloat(stock.Symbol);
                            stock.ShareFloat = shareFloat;
                        }

                        var nameTag = tr.SelectSingleNode(".//td[@aria-label='Name']");
                        if (nameTag != null)
                        {
                            stock.Name = nameTag.InnerHtml;
                        }
                        var priceTag = tr.SelectSingleNode(".//td[@aria-label='Price (Intraday)']/fin-streamer");
                        if (priceTag != null)
                        {
                            stock.Price = Convert.ToDouble(priceTag.InnerHtml);
                        }
                        var peRatioTag = tr.SelectSingleNode(".//td[@aria-label='PE Ratio (TTM)']");
                        if (peRatioTag != null)
                        {
                            double peRatio;
                            if (double.TryParse(peRatioTag.InnerHtml, out peRatio))
                            {
                                stock.PERatio = peRatio;
                            }
                        }
                        var marketCapTag = tr.SelectSingleNode(".//td[@aria-label='Market Cap']/fin-streamer");
                        if (marketCapTag != null)
                        {
                            string marketCapString = marketCapTag.InnerHtml;
                            if (marketCapString.EndsWith("B"))
                            {
                                marketCapString = marketCapString.Substring(0, marketCapString.Length - 1);
                                stock.MarketCap = Convert.ToDouble(marketCapString) * 1000000000;
                            }
                            else if (marketCapString.EndsWith("T"))
                            {
                                marketCapString = marketCapString.Substring(0, marketCapString.Length - 1);
                                stock.MarketCap = Convert.ToDouble(marketCapString) * 1000000000000;
                            }
                            else
                            {
                                stock.MarketCap = Convert.ToDouble(marketCapString);
                            }
                        }

                        var volumeTag = tr.SelectSingleNode(".//td[@aria-label='Volume']/fin-streamer");
                        if (volumeTag != null)
                        {
                            string volumeString = volumeTag.InnerHtml;
                            if (volumeString.EndsWith("B"))
                            {
                                volumeString = volumeString.Substring(0, volumeString.Length - 1);
                                stock.Volume = Convert.ToDouble(volumeString) * 1000000000;
                            }
                            else if (volumeString.EndsWith("T"))
                            {
                                volumeString = volumeString.Substring(0, volumeString.Length - 1);
                                stock.Volume = Convert.ToDouble(volumeString) * 1000000000000;
                            }
                            else if (volumeString.EndsWith("M"))
                            {
                                volumeString = volumeString.Substring(0, volumeString.Length - 1);
                                stock.Volume = Convert.ToDouble(volumeString) * 1000000;
                            }
                            else
                            {
                                stock.Volume = Convert.ToDouble(volumeString);
                            }
                        }

                        var regularMarketChangePercentTag = tr.SelectSingleNode(".//td[@aria-label='% Change']/fin-streamer");
                        if (regularMarketChangePercentTag != null)
                        {
                            double regularMarketChangePercent = regularMarketChangePercentTag.Attributes["value"].Value == "0" ? 0 : Convert.ToDouble(regularMarketChangePercentTag.Attributes["value"].Value);
                            stock.PercentageChange = regularMarketChangePercent;
                        }

                        stocks.Add(stock);
                    }

                    // foreach (var stock in stocks)
                    // {
                    //     Console.WriteLine(stock.Symbol);
                    // }

                }

            }
            else
            {
                return (stocks, stocks.Count);
            }

            return await GetSymbolFromHtml(screenerId, 100, offset += 100, stocks);
        }

        public async Task<byte[]> GetStockMarketCapBarChart(string screenerId)
        {
            var data = await GetSymbolFromHtml(screenerId, 100, 0, new List<Stock>());
            var stocksData = data.stocks;

            var sortedData = stocksData.OrderByDescending(d => d.MarketCap).ToList();
            var model = new PlotModel { Title = "Market Capitalization of Companies (in billion USD)" };
            var barSeries = new BarSeries
            {
                ItemsSource = sortedData.Select(d => new BarItem { Value = (double)(d.MarketCap ?? 0) / 1e9 }),
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:.00}"
            };

            model.Series.Add(barSeries);
            model.Axes.Add(new CategoryAxis { Position = AxisPosition.Left, Key = "SymbolAxis", ItemsSource = sortedData.Select(d => d.Symbol) });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, AbsoluteMinimum = 0, MaximumPadding = 0.06 });


            //export to png and return the binary data
            var exporter = new PngExporter { Width = 1000, Height = data.count * 15 };
            using (var stream = new MemoryStream())
            {
                exporter.Export(model, stream);
                var bytes = stream.ToArray();
                return bytes;
            }

        }





        public async void ScrapShareFloatBySymbol(Stock stock)
        {
            // get share float from key statistic page
            var keyStatisticHtml = await GetYahooStockKeyStatisticHtml(stock.Symbol);
            var keyStatisticHtmlDocument = new HtmlDocument();
            keyStatisticHtmlDocument.LoadHtml(keyStatisticHtml);
            var tbody2s = keyStatisticHtmlDocument.DocumentNode.SelectNodes("//tbody");
            var tbody2 = tbody2s[2];

            if (tbody2 != null && tbody2.InnerHtml != string.Empty)
            {
                var trs2 = tbody2.SelectNodes(".//tr");
                if (trs2 != null)
                {
                    foreach (var tr2 in trs2)
                    {
                        var titleTag = tr2.SelectSingleNode(".//td/span");
                        Console.WriteLine(titleTag?.InnerHtml);
                        if (titleTag?.InnerHtml == "Shares Outstanding")
                        {
                            // select  second td from tr
                            var shareFloatString = tr2.SelectSingleNode(".//td[2]")?.InnerHtml;


                            if (shareFloatString.EndsWith("B"))
                            {
                                shareFloatString = shareFloatString.Substring(0, shareFloatString.Length - 1);
                                stock.ShareFloat = Convert.ToDouble(shareFloatString) * 1000000000;
                            }
                            else if (shareFloatString.EndsWith("T"))
                            {
                                shareFloatString = shareFloatString.Substring(0, shareFloatString.Length - 1);
                                stock.ShareFloat = Convert.ToDouble(shareFloatString) * 1000000000000;
                            }
                            else if (shareFloatString.EndsWith("M"))
                            {
                                shareFloatString = shareFloatString.Substring(0, shareFloatString.Length - 1);
                                stock.ShareFloat = Convert.ToDouble(shareFloatString) * 1000000;
                            }
                            else
                            {
                                double.TryParse(shareFloatString, out double shareFloat);
                                // break if parse fail
                                if (shareFloat == 0)
                                {
                                    break;
                                }
                                stock.ShareFloat = Convert.ToDouble(shareFloatString);
                            }
                        }
                    }
                }
            }
        }
    }
}

