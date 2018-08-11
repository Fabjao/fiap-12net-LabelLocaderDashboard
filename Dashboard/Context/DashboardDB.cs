using Microsoft.EntityFrameworkCore;
using Dashboard.Models;

namespace Dashboard.Context
{
    public class DashboardDB : DbContext
    {
        private object options;

        public DashboardDB(object options)
        {
            this.options = options;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<FoodRestriction> FoodRestrictions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("DashboardDB");
        }
    }
}
