namespace LoanManagementSystem.Models.Users;

public abstract class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public UserRole Role { get; protected set; }

    public abstract bool HasPermission(Permission permission);
    public abstract string GetDashboardFormName();
}

public enum UserRole
{
    Admin,
    LoanOfficer,
    Cashier,
    Borrower
}

[Flags]
public enum Permission
{
    None = 0,
    
    // View Permissions
    ViewDashboard = 1,
    ViewLoans = 2,
    ViewCustomers = 4,
    ViewReports = 8,
    ViewProducts = 16,
    
    // Loan Operations
    CreateLoanApplication = 32,
    ApproveLoan = 64,
    RejectLoan = 128,
    ReleaseLoan = 256,
    
    // Payment Operations
    ProcessPayment = 512,
    GenerateReceipt = 1024,
    ViewDailyCollections = 2048,
    
    // Management
    ManageUsers = 4096,
    ManageCustomers = 8192,
    ManageProducts = 16384,
    ConfigureSystem = 32768,
    
    // Special
    Override = 65536,
    ViewOwnLoansOnly = 131072, // For Borrowers
    
    // Combined permissions for convenience
    AllViewPermissions = ViewDashboard | ViewLoans | ViewCustomers | ViewReports | ViewProducts,
    AllLoanOperations = CreateLoanApplication | ApproveLoan | RejectLoan | ReleaseLoan,
    AllPaymentOperations = ProcessPayment | GenerateReceipt | ViewDailyCollections,
    AllManagement = ManageUsers | ManageCustomers | ManageProducts | ConfigureSystem,
    
    // Full access
    All = AllViewPermissions | AllLoanOperations | AllPaymentOperations | AllManagement | Override
}
