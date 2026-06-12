using Microsoft.EntityFrameworkCore;
using Model.Admin;

namespace MyProtfolio.API.Data{
    public class PortfolioDbContext : DbContext
    {
        public DbSet<Admin> Admins {get; set;}
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
        }
    }
}