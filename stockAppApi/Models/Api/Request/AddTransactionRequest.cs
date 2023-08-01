using stockAppApi.Entities;

namespace stockAppApi.Models.Api.Request
{
    public class AddTransactionRequest
    {
        public int? StockId { get; set; }
        public TransactionType? Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}