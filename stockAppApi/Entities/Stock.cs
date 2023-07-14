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
    public string Symbol { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Date")]
    public DateTime Date { get; set; }

    [Column("Open")]
    public decimal Open { get; set; }

    [Column("High")]
    public decimal High { get; set; }

    [Column("Low")]
    public decimal Low { get; set; }

    [Column("Close")]
    public decimal Close { get; set; }

    [Column("Volume")]
    public decimal Volume { get; set; }
}