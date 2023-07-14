using System;
using FluentValidation;
using stockAppApi.BLL.Validators;
using stockAppApi.Entities;
using stockAppApi.Models.Api.Request;

namespace stockAppApi.BLL
{
    // create interface for the stock service
    public interface IStockService
    {
        Stock InsertStock(InsertStockRequest request);
        List<Stock> ListStocks();
    }
    public class StockService : IStockService
    {
        private readonly DataContext _dbContext;
        public StockService(DataContext context)
        {
            _dbContext = context;
        }

        public Stock InsertStock(InsertStockRequest request)
        {
            //validate the request
            var validator = new InsertStockValidator();
            validator.ValidateAndThrow(request);

            // create a new stock object
            var stock = new Stock
            {
                Symbol = request.Symbol,
                Name = request.Name,
                Date = request.Date,
                Open = request.Open,
                High = request.High,
                Low = request.Low,
                Close = request.Close,
                Volume = request.Volume
            };
            _dbContext.Stocks.Add(stock);
            _dbContext.SaveChanges();
            return stock;
        }

        // list stocks from the database
        public List<Stock> ListStocks()
        {
            var stocks = _dbContext.Stocks.ToList();
            return stocks;
        }
    }
}

