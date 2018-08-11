using Microsoft.EntityFrameworkCore;

namespace Dashboard.Context
{
    public class DashboardDB : DbContext
    {
        private object options;

        public DashboardDB(object options)
        {
            this.options = options;
        }

        public DbSet<Mock.OrderChanged> MockedOrderChanged { get; set; }
        public DbSet<GeekBurger.Orders.Contract.Messages.OrderChangedMessage> OrderChanged { get; set; }
        public DbSet<GeekBurger.Users.Contract.UserFoodRestriction> UserFoodRestriction { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("DashboardDB");
        }
    }
}
