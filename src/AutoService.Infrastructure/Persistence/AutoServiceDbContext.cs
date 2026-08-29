using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Persistence;

public class AutoServiceDbContext : DbContext
{
    public AutoServiceDbContext(DbContextOptions<AutoServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ServiceAppointment> ServiceAppointments => Set<ServiceAppointment>();
    public DbSet<ServiceRecord> ServiceRecords => Set<ServiceRecord>();
    public DbSet<SparePart> SpareParts => Set<SparePart>();
    public DbSet<ServiceRecordPart> ServiceRecordParts => Set<ServiceRecordPart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SparePart>()
            .HasIndex(sparePart => sparePart.Sku)
            .IsUnique();

        modelBuilder.Entity<SparePart>()
            .Property(sparePart => sparePart.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ServiceRecord>()
            .Property(serviceRecord => serviceRecord.LaborCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ServiceRecordPart>()
            .HasKey(item => new { item.ServiceRecordId, item.SparePartId });

        modelBuilder.Entity<ServiceRecordPart>()
            .Property(item => item.UnitPriceAtUse)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ServiceRecordPart>()
            .HasOne(item => item.ServiceRecord)
            .WithMany(serviceRecord => serviceRecord.Parts)
            .HasForeignKey(item => item.ServiceRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceRecordPart>()
            .HasOne(item => item.SparePart)
            .WithMany(sparePart => sparePart.ServiceRecordParts)
            .HasForeignKey(item => item.SparePartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
