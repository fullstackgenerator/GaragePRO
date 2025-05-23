using GaragePRO.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaragePRO.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<ServiceDetail> ServiceDetails { get; set; }
    public DbSet<PartCatalog> PartCatalogs { get; set; }
    public DbSet<PartUsed> PartUsed { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.VIN)
            .IsUnique();
    }
}