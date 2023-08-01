// create a stock model class
namespace stockAppApi.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("StockHistory")]
public class StockHistory
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    [Column("StockId")]
    [ForeignKey("Stock")]
    public int StockId { get; set; }
    [Column("Date")]
    public DateTime Date { get; set; }
    [Column("ClosePrice")]
    public double ClosePrice { get; set; }
    [Column("Volume")]
    public int Volumn { get; set; }

    public Stock Stock { get; set; }
}