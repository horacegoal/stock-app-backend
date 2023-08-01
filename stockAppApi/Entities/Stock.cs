// create a stock model class
namespace stockAppApi.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Stocks")]
public class Stock
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Symbol")]
    public string? Symbol { get; set; }

    [Column("Name")]
    public string? Name { get; set; }

    [Column("Date")]
    public DateTime Date { get; set; }


    [Column("Price")]
    public double? Price { get; set; }

    // Add Market Cap column field
    [Column("MarketCap")]
    public double? MarketCap { get; set; }

    // Add p/e ratio column field
    [Column("PERatio")]
    public double? PERatio { get; set; }

    // Add volumn
    [Column("Volume")]
    public double? Volume { get; set; }

    // add percentage change
    [Column("PercentageChange")]
    public double? PercentageChange { get; set; }

    //add share float
    [Column("ShareFloat")]
    public double? ShareFloat { get; set; }

    public int? AverageDailyVolume10Day { get; set; }

    //json ignore
    [System.Text.Json.Serialization.JsonIgnore]
    public List<Transaction> Transactions { get; set; }

    public List<StockHistory> StockHistories { get; set; }
    [NotMapped]
    public int ShareCount { get; set; }
    [NotMapped]
    public double CostPerShare { get; set; }

}