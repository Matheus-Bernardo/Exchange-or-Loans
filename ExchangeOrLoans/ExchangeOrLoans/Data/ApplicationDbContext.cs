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
        // Tables
        public DbSet<User> Users { get; set; }
        
    }
}