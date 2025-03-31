using Microsoft.EntityFrameworkCore;

namespace RentalManagementSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }  // Thêm bảng Maintenance
        public DbSet<Component> Components { get; set; }  // Thêm bảng Component
        public DbSet<MComponent> MComponents { get; set; }  // Thêm bảng trung gian Machine - Component

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa khóa chính cho bảng trung gian MComponent
            modelBuilder.Entity<MComponent>()
                .HasKey(mc => mc.MComponentID);

            // Thiết lập quan hệ giữa Machine và MComponent
            modelBuilder.Entity<MComponent>()
                .HasOne(mc => mc.Machine)
                .WithMany(m => m.MComponents)
                .HasForeignKey(mc => mc.MachineID)
                .OnDelete(DeleteBehavior.Cascade);

            // Thiết lập quan hệ giữa Component và MComponent
            modelBuilder.Entity<MComponent>()
                .HasOne(mc => mc.Component)
                .WithMany(c => c.MComponents)
                .HasForeignKey(mc => mc.ComponentID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
