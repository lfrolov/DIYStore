using DIYDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DIYDb
{
    public partial class DIYDbContext : DbContext
    {
        #region Tables
        public DbSet<Product> Products { get; set; }
        public DbSet<Unit> Units { get; set; }
        #endregion

        public DIYDbContext(DbContextOptions<DIYDbContext> options) : base(options)
        {
            //Database.EnsureDeleted(); // UnComment for first use init db with data
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.IniDbData(modelBuilder);
        }
    }
}
