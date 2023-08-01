using Microsoft.EntityFrameworkCore;
using stockAppApi.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(Configuration.GetConnectionString("MyDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Stock>()
            .Property(s => s.Date)
            .HasDefaultValueSql("datetime('now')");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        // add relationshio between stock's id and transaction StockId field
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Stock)
            .WithMany(s => s.Transactions)
            .HasForeignKey(t => t.StockId);

        modelBuilder.Entity<StockHistory>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();


        modelBuilder.Entity<StockHistory>()
            .HasOne(s => s.Stock)
            .WithMany(s => s.StockHistories)
            .HasForeignKey(s => s.StockId);

    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<StockHistory> StockHistories { get; set; }
}