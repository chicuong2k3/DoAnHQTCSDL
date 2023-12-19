using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace DataModels
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //key for AppointmentSchedule
            builder.Entity<AppointmentSchedule>()
                .HasKey(x => new { x.DentistId, x.StartTime });
            // n-n relationship
            builder.Entity<Medicine_MedicalRecord>()
                .HasKey(x => new { x.MedicineId, x.MedicalRecordId, x.SequenceNumber });
            builder.Entity<Medicine_MedicalRecord>()
                .HasOne(m => m.Medicine)
                .WithMany(mmr => mmr.Medicine_MedicalRecords)
                .HasForeignKey(mmr => mmr.MedicineId);
            builder.Entity<Medicine_MedicalRecord>()
                .HasOne(m => m.MedicalRecord)
                .WithMany(mmr => mmr.Medicine_MedicalRecords)
                .HasForeignKey(mmr => new { mmr.MedicalRecordId, mmr.SequenceNumber });

            //
            builder.Entity<MedicalRecord>()
                .HasKey(x => new { x.Id, x.SequenceNumber });

            builder.Entity<MedicalRecord>()
            .HasOne(mr => mr.Customer)
            .WithMany()
            .HasForeignKey(mr => mr.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MedicalRecord>()
                .HasOne(mr => mr.CreatedByDentist)
                .WithMany()
                .HasForeignKey(mr => mr.CreatedByDentistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MedicalRecord>()
                .HasOne(mr => mr.ExamDentist)
                .WithMany()
                .HasForeignKey(mr => mr.ExamDentistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Dentist>()
                .HasOne(d => d.Account)
                .WithMany()
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.NoAction);




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

            Randomizer.Seed = new Random(8675390);

        }


        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dentist> Dentists { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppointmentSchedule> AppointmentSchedules { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineInventory> MedicineInventories { get; set; }
        public DbSet<Medicine_MedicalRecord> Medicine_MedicalRecords { get; set; }
    }
}