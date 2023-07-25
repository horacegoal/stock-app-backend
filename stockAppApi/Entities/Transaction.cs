// create a stock model class
namespace stockAppApi.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum TransactionType
{
    Buy,
    Sell
}

[Table("Transaction")]
public class Transaction
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("StockId")]
    [ForeignKey("Stock")]
    public int? StockId { get; set; }

    [Column("Type")]
    public TransactionType? Type { get; set; }

    [Column("Quantity")]
    public int Quantity { get; set; }


    [Column("Price")]
    public double Price { get; set; }

    // Add Market Cap column field
    [Column("DateCreated")]
    public DateTime DateCreated { get; set; }

    public Stock Stock { get; set; }


}