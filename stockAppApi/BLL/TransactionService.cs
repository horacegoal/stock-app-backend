using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using stockAppApi.BLL.Validators;
using stockAppApi.Entities;
using stockAppApi.Models.Api.Request;

namespace stockAppApi.BLL
{

    public class ProfitByMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double Profit { get; set; }
    }

    public class ProfitData
    {
        public List<ProfitByMonth> Data { get; set; }
        public double TotalProfit { get; set; }
    }
    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(AddTransactionRequest request);
        Transaction translateTransactionMessage(string message);
        List<Transaction> ListTransactions();
        double calculateTotalProfitFromSellTransactions();
        double calculateProfitFromSellTransactionsByMonth(int month, int year);

        ProfitData ListProfitByMonth();
    }

    public class TransactionService : ITransactionService
    {
        private readonly DataContext _dbContext;
        public TransactionService(DataContext context)
        {
            _dbContext = context;
        }

        // list transactions
        public List<Transaction> ListTransactions()
        {
            var transactionsWithStocks = _dbContext.Transactions
                                                .Include(t => t.Stock) // Eager load the related Stock.
                                                .OrderByDescending(t => t.TransactionDate) // Sort by transactionDate in descending order.
                                                .ToList();

            return transactionsWithStocks.Select(x => new Transaction
            {
                Id = x.Id,
                StockId = x.StockId,
                Type = x.Type,
                Quantity = x.Quantity,
                Price = x.Price,
                TransactionDate = x.TransactionDate,
                Stock = x.Stock // Safely access Symbol with null check.
            }).ToList();
        }

        public async Task<Transaction> AddTransaction(AddTransactionRequest request)
        {
            var validator = new AddTransactionValidator();
            validator.ValidateAndThrow(request);


            var transaction = new Transaction
            {
                StockId = request.StockId,
                Type = request.Type,
                Quantity = request.Quantity,
                Price = request.Price,
                TransactionDate = request.TransactionDate
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            return transaction;
        }

        public Transaction translateTransactionMessage(string message)
        {
            // Tue, 14 Feb at 11:41 PM  滙豐: 買入 20 股 GOOGL， 成交價 USD92.96
            // Tue, 6 Jun at 10:22 PM 滙豐: 沽出 26 股 LAZR， 成交價 USD7.37
            // remove all whitespace inside message
            message = Regex.Replace(message, @"\s+", "");
            Console.WriteLine(message);

            int atIndex = message.IndexOf("at");
            //avoid get index of Sat
            if (message[atIndex - 1] == 'S')
            {
                atIndex = message.IndexOf("at", atIndex + 1);
            }

            string date = message.Substring(message.IndexOf(",") + 1, atIndex - message.IndexOf(",") - 1) + " 2023";
            Console.WriteLine(date);
            string time = message.Substring(atIndex + 2, message.IndexOf("滙") - atIndex - 2);
            Console.WriteLine(time);


            TransactionType? type = message.Contains("買入") ? TransactionType.Buy : message.Contains("沽出") ? TransactionType.Sell : null;
            Console.WriteLine(type);

            int quantity = int.Parse(message.Substring(message.IndexOf(type == TransactionType.Buy ? "買入" : "沽出") + 2, message.IndexOf("股") - message.IndexOf(type == TransactionType.Buy ? "買入" : "沽出") - 2));
            Console.WriteLine(quantity);

            double price = Double.Parse(message.Substring(message.IndexOf("USD") + 3, message.Length - message.IndexOf("USD") - 3));
            Console.WriteLine(price);

            DateTime transactionDate = DateTime.Parse(date + " " + time);
            Console.WriteLine(transactionDate);

            string symbol = message.Substring(message.IndexOf("股") + 1, message.IndexOf("，") - message.IndexOf("股") - 1);
            Console.WriteLine(symbol);

            int? stockId = (_dbContext.Stocks.Where(x => x.Symbol == symbol).FirstOrDefault()?.Id) ?? throw new Exception("Stock not found");
            Console.WriteLine(stockId);

            return new Transaction
            {
                StockId = stockId,
                Type = type,
                Quantity = quantity,
                Price = price,
                TransactionDate = transactionDate
            };

        }

        public double calculateTotalProfitFromSellTransactions()
        {
            var transactions = _dbContext.Transactions.Where(x => x.Type == TransactionType.Sell).ToList();
            double totalProfit = 0;
            transactions.ForEach(x =>
            {
                totalProfit += calculateProfitFromSell(x);
            });
            return totalProfit;
        }

        public double calculateProfitFromSellTransactionsByMonth(int month, int year)
        {
            var transactions = _dbContext.Transactions.Where(x => x.Type == TransactionType.Sell && x.TransactionDate.Month == month && x.TransactionDate.Year == year).ToList();
            double totalProfit = 0;
            transactions.ForEach(x =>
            {
                totalProfit += calculateProfitFromSell(x);
            });
            return totalProfit;
        }
        public double calculateProfitFromSell(Transaction sellTransaction)
        {
            if (sellTransaction.Type != TransactionType.Sell)
            {
                throw new Exception("This is not a sell transaction");
            }
            // calculate cost per stock 
            string symbol = _dbContext.Stocks.Where(x => x.Id == sellTransaction.StockId).FirstOrDefault().Symbol;
            // get all transactions for this stock before sellTransaction
            var transactions = _dbContext.Transactions.Where(x => x.Stock.Symbol == symbol && x.TransactionDate < sellTransaction.TransactionDate).ToList();
            double costPerShare = 0;
            int totalShares = 0;
            transactions.ForEach(x =>
            {
                if (x.Type == TransactionType.Buy)
                {
                    costPerShare = (costPerShare * totalShares + x.Price * x.Quantity) / (totalShares + x.Quantity);
                    totalShares += x.Quantity;
                }

                else if (x.Type == TransactionType.Sell)
                {
                    totalShares -= x.Quantity;
                }
            });
            // calculate profit
            double profit = sellTransaction.Price * sellTransaction.Quantity - costPerShare * sellTransaction.Quantity;
            return profit;
        }


        public ProfitData ListProfitByMonth()
        {
            DateTime oldestTransDate = _dbContext.Transactions.OrderBy(x => x.TransactionDate).FirstOrDefault().TransactionDate;
            DateTime now = DateTime.Now;

            List<ProfitByMonth> profitByMonthList = new List<ProfitByMonth>();
            double totalProfit = 0;
            while (oldestTransDate.Month <= now.Month && oldestTransDate.Year <= now.Year)
            {
                int month = oldestTransDate.Month;
                int year = oldestTransDate.Year;
                double profit = calculateProfitFromSellTransactionsByMonth(month, year);
                totalProfit += profit;
                profitByMonthList.Add(new ProfitByMonth
                {
                    Month = month,
                    Year = year,
                    Profit = profit
                });
                oldestTransDate = oldestTransDate.AddMonths(1);
            }
            return new ProfitData
            {
                Data = profitByMonthList,
                TotalProfit = totalProfit
            };

        }
    }


}