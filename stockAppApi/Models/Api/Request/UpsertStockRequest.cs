using System;
namespace stockAppApi.Models.Api.Request
{
    public class UpsertStockRequest
    {
        public string? Symbol { get; set; }

        public string? Name { get; set; }

        public DateTime Date { get; set; }

        public double? Price { get; set; }

        public double? MarketCap { get; set; }

        public double? PERatio { get; set; }

        public double? Volume { get; set; }

        public double? PercentageChange { get; set; }

        public double? ShareFloat { get; set; }

        public int? AverageDailyVolume10Day { get; set; }

    }
}

