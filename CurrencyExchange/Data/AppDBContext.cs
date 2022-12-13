using CurrencyExchange.Model;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<CurrencyExchangeDBModel> CurrencyExchanges { get; set; } 
    }
}
