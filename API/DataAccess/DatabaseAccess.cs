using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Models.Mappings;
namespace API.DataAccess
{
    public class DatabaseAccess : DbContext
    {
        public DatabaseAccess(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new ConfigurationMapping());
        }
    }
}