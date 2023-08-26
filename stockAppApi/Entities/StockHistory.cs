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
    [Required]
    public int StockId { get; set; }
    [Required]
    [Column("Date")]
    public DateTime Date { get; set; }
    [Required]
    [Column("ClosePrice")]
    public double ClosePrice { get; set; }
    [Required]
    [Column("Volume")]
    public long? Volumn { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public Stock Stock { get; set; }
}