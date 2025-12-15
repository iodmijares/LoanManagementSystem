using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Models;
using LoanManagementSystem.Models.Users;
using LoanManagementSystem.Utilities;

namespace LoanManagementSystem.Data;

public class LoanDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CoMaker> CoMakers { get; set; }
    public DbSet<LoanProduct> LoanProducts { get; set; }
    public DbSet<LoanApplication> LoanApplications { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<PaymentSchedule> PaymentSchedules { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    // User tables - using TPH inheritance
    public DbSet<User> Users { get; set; }

    private readonly string _connectionString;

    public LoanDbContext()
    {
        _connectionString = "Server=(localdb)\\mssqllocaldb;Database=LoanManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true";
    }

    public LoanDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User inheritance (TPH - Table Per Hierarchy)
        modelBuilder.Entity<User>()
            .HasDiscriminator(u => u.Role)
            .HasValue<Admin>(UserRole.Admin)
            .HasValue<LoanOfficer>(UserRole.LoanOfficer)
            .HasValue<Cashier>(UserRole.Cashier);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CustomerId).IsUnique();
            entity.Property(e => e.MonthlyIncome).HasPrecision(18, 2);
            entity.Property(e => e.CreditScore).HasPrecision(5, 2);
        });

        // CoMaker configuration
        modelBuilder.Entity<CoMaker>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MonthlyIncome).HasPrecision(18, 2);
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.CoMakers)
                .HasForeignKey(e => e.CustomerId);
        });

        // Loan Product configuration
        modelBuilder.Entity<LoanProduct>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ProductCode).IsUnique();
            entity.Property(e => e.AnnualInterestRate).HasPrecision(5, 2);
            entity.Property(e => e.ServiceChargePercent).HasPrecision(5, 2);
            entity.Property(e => e.ProcessingFeeFixed).HasPrecision(18, 2);
            entity.Property(e => e.MinimumAmount).HasPrecision(18, 2);
            entity.Property(e => e.MaximumAmount).HasPrecision(18, 2);
            entity.Property(e => e.PenaltyRatePerDay).HasPrecision(5, 4);
        });

        // Loan Application configuration
        modelBuilder.Entity<LoanApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApplicationNumber).IsUnique();
            entity.Property(e => e.RequestedAmount).HasPrecision(18, 2);
            entity.Property(e => e.ComputedMonthlyPayment).HasPrecision(18, 2);
            entity.Property(e => e.ComputedTotalInterest).HasPrecision(18, 2);
            entity.Property(e => e.ComputedTotalPayable).HasPrecision(18, 2);
            entity.Property(e => e.ServiceCharge).HasPrecision(18, 2);
            entity.Property(e => e.ProcessingFee).HasPrecision(18, 2);
            entity.Property(e => e.NetProceedsAmount).HasPrecision(18, 2);
            entity.Property(e => e.CollateralValue).HasPrecision(18, 2);
            entity.Property(e => e.CreditScoreAtApplication).HasPrecision(5, 2);
            entity.Property(e => e.ApprovedAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.LoanProduct)
                .WithMany()
                .HasForeignKey(e => e.LoanProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CoMaker)
                .WithMany()
                .HasForeignKey(e => e.CoMakerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Loan configuration
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LoanNumber).IsUnique();
            entity.Property(e => e.PrincipalAmount).HasPrecision(18, 2);
            entity.Property(e => e.MonthlyPayment).HasPrecision(18, 2);
            entity.Property(e => e.TotalInterest).HasPrecision(18, 2);
            entity.Property(e => e.TotalPayable).HasPrecision(18, 2);
            entity.Property(e => e.ServiceCharge).HasPrecision(18, 2);
            entity.Property(e => e.ProcessingFee).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingPrincipal).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingInterest).HasPrecision(18, 2);
            entity.Property(e => e.TotalPenalties).HasPrecision(18, 2);
            entity.Property(e => e.TotalPaid).HasPrecision(18, 2);
            entity.Property(e => e.AnnualInterestRate).HasPrecision(5, 2);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Loans)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.LoanProduct)
                .WithMany()
                .HasForeignKey(e => e.LoanProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.LoanApplication)
                .WithOne(a => a.Loan)
                .HasForeignKey<Loan>(e => e.LoanApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment Schedule configuration
        modelBuilder.Entity<PaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrincipalDue).HasPrecision(18, 2);
            entity.Property(e => e.InterestDue).HasPrecision(18, 2);
            entity.Property(e => e.TotalDue).HasPrecision(18, 2);
            entity.Property(e => e.PrincipalPaid).HasPrecision(18, 2);
            entity.Property(e => e.InterestPaid).HasPrecision(18, 2);
            entity.Property(e => e.PenaltyPaid).HasPrecision(18, 2);
            entity.Property(e => e.TotalPaid).HasPrecision(18, 2);
            entity.Property(e => e.PrincipalBalance).HasPrecision(18, 2);

            entity.HasOne(e => e.Loan)
                .WithMany(l => l.PaymentSchedules)
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ReceiptNumber).IsUnique();
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.PrincipalPortion).HasPrecision(18, 2);
            entity.Property(e => e.InterestPortion).HasPrecision(18, 2);
            entity.Property(e => e.PenaltyPortion).HasPrecision(18, 2);
            entity.Property(e => e.ChargesPortion).HasPrecision(18, 2);

            entity.HasOne(e => e.Loan)
                .WithMany(l => l.Payments)
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaymentSchedule)
                .WithMany()
                .HasForeignKey(e => e.PaymentScheduleId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed default admin user
        modelBuilder.Entity<Admin>().HasData(
            new Admin
            {
                Id = 1,
                Username = "admin",
                PasswordHash = PasswordHelper.HashPassword("admin123"),
                FullName = "System Administrator",
                Email = "admin@lms.com",
                Phone = "09171234567",
                CreatedAt = DateTime.Now,
                IsActive = true
            });

        // Seed default loan products
        modelBuilder.Entity<LoanProduct>().HasData(
            new LoanProduct
            {
                Id = 1,
                ProductCode = "PL001",
                ProductName = "Personal Loan",
                Description = "General purpose personal loan",
                LoanType = LoanType.Personal,
                AnnualInterestRate = 12M,
                InterestMethod = InterestCalculationMethod.DiminishingBalance,
                ServiceChargePercent = 2M,
                ProcessingFeeFixed = 500M,
                MinimumAmount = 10000M,
                MaximumAmount = 500000M,
                AvailableTermsMonths = "6,12,18,24",
                PenaltyRatePerDay = 0.1M,
                GracePeriodDays = 3,
                RequiresCoMaker = true,
                IsActive = true
            },
            new LoanProduct
            {
                Id = 2,
                ProductCode = "EL001",
                ProductName = "Emergency Loan",
                Description = "Quick emergency financial assistance",
                LoanType = LoanType.Emergency,
                AnnualInterestRate = 15M,
                InterestMethod = InterestCalculationMethod.FlatRate,
                ServiceChargePercent = 3M,
                ProcessingFeeFixed = 300M,
                MinimumAmount = 5000M,
                MaximumAmount = 100000M,
                AvailableTermsMonths = "3,6,12",
                PenaltyRatePerDay = 0.15M,
                GracePeriodDays = 2,
                RequiresCoMaker = false,
                IsActive = true
            },
            new LoanProduct
            {
                Id = 3,
                ProductCode = "SL001",
                ProductName = "Salary Loan",
                Description = "Loan against monthly salary",
                LoanType = LoanType.Salary,
                AnnualInterestRate = 10M,
                InterestMethod = InterestCalculationMethod.DiminishingBalance,
                ServiceChargePercent = 1.5M,
                ProcessingFeeFixed = 200M,
                MinimumAmount = 5000M,
                MaximumAmount = 200000M,
                AvailableTermsMonths = "6,12,18,24",
                PenaltyRatePerDay = 0.1M,
                GracePeriodDays = 5,
                RequiresCoMaker = false,
                IsActive = true
            });
    }
}
