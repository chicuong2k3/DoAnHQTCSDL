using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataModels
{
    public class AppDbContext : IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
            });

            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();

            string adminRoleId = "9b542a62-e9c0-4073-aa96-4332230e32bc";
            string customerRoleId = "4fa697ec-6c32-4116-8c01-3a202463703f";
            string dentistRoleId = "39934cbd-ca9b-43a2-9028-192615dc638a";
            string employeeRoleId = "31234cbd-ca9b-45a2-9028-192615dc638a";
            string adminId = "c6647262-ef40-40b3-af33-f89f80d35378";

            builder.Entity<IdentityRole>().HasData(new IdentityRole 
            { 
                Id = adminRoleId,
                Name = "Admin", 
                NormalizedName = "ADMIN"
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = customerRoleId,
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = dentistRoleId,
                Name = "Dentist",
                NormalizedName = "DENTIST"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = employeeRoleId,
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            });

            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = adminId,
                UserName = "admin123",
                NormalizedUserName = "ADMIN123",
                PasswordHash = hasher.HashPassword(null, "admin123")
            });

            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                
                RoleId = adminRoleId,
                UserId = adminId
            });
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dentist> Dentists { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
