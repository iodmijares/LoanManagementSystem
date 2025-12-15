namespace LoanManagementSystem.Models;

public class Customer
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;

    // Personal Information
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string CivilStatus { get; set; } = string.Empty;

    // Contact Information
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Identification
    public string PrimaryIdType { get; set; } = string.Empty;
    public string PrimaryIdNumber { get; set; } = string.Empty;

    // Employment Information
    public string EmploymentStatus { get; set; } = string.Empty;
    public string EmployerName { get; set; } = string.Empty;
    public string EmployerAddress { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public int YearsEmployed { get; set; }

    // Classification
    public CustomerClassification Classification { get; set; } = CustomerClassification.Regular;
    public decimal CreditScore { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

    // Navigation properties
    public virtual List<Loan> Loans { get; set; } = [];
    public virtual List<CoMaker> CoMakers { get; set; } = [];
}

public enum CustomerClassification
{
    Regular,
    VIP,
    Blacklisted
}
