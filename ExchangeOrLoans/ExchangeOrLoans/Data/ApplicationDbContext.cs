using ExchangeOrLoans.models;
using Microsoft.EntityFrameworkCore;
namespace ExchangeOrLoans.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Tables of dataBase
        public DbSet<User> User { get; set; }
        
    }
}