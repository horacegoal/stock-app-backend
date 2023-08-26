using System;
namespace stockAppApi.Models.Api.Request
{
    public class InsertStockHistoryRequest
    {
        public int StockId { get; set; }
        public DateTime Date { get; set; }
        public double ClosePrice { get; set; }
        public long Volumn { get; set; }


    }
}

