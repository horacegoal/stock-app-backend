using System;
namespace stockAppApi.Models.Api.Request
{
    public class InsertStockRequest
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }
    }
}

