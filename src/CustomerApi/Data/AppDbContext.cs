namespace CustomerApi.Data;

using Microsoft.EntityFrameworkCore;
using CustomerApi.Models;


public class AppDbContext : DbContext {

    public DbSet<Customer> Customers {get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

    }
}