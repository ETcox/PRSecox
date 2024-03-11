using Microsoft.EntityFrameworkCore;

namespace PRSecox.Models
{
    public class PRSDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }   
        public DbSet<LineItem> LineItems { get; set; }  

        //this passes in the connection string
        public PRSDbContext(DbContextOptions<PRSDbContext> options) : base(options)

        {
            //custom stuff goes here
        }
    }
}
