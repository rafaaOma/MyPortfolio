using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Model.Admin;


namespace MyProtfolio.API.Data{
    public class PortfolioDbContext : DbContext
    {
        public DbSet<Admin> Admins {get; set;} //admin info
        public DbSet<Skill> Skills { get; set; } //skills table
        public DbSet<Project> Projects { get; set; } //project table
        public DbSet<ProjectImage> ProjectImages { get; set; } //projec timages

        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    UserName = "rafa",
                    Password = "$2a$11$yH99rJMZqF951xuZHA0MdeZlYwf4CIPQ3UG1W8fU5lRBjI9PVlfv6" // Initial admin for first setup only
                }
            );
        }
    }
}