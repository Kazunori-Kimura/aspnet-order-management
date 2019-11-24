using System.Data.Entity;

namespace OrderManagement.Models
{
    public class OrderContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Detail> Details { get; set; }
    }
}